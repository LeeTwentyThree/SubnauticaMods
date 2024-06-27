using System.Collections.Generic;
using System.Collections;
using ModStructureHelperPlugin.EntityHandling;
using ModStructureHelperPlugin.Mono;
using UnityEngine;
using UnityEngine.UI;
using UWE;

namespace ModStructureHelperPlugin.UI;

public class UIEntityWindow : MonoBehaviour
{
    public static UIEntityWindow Main { get; private set; }

    public GameObject buttonPrefab;
    public RectTransform contentParent;
    public Transform poolParent;

    public RectTransform directoryButtonsParent;
    public GameObject directoryButtonPrefab;
    public GameObject directorySeparatorPrefab;

    public InputField searchBar;

    private List<string> visitedFolderHistory = new List<string>();
    private string lastActiveFolderPath;
    private string activeFolderPath;

    private bool showingSearchResults;

    private List<EntityBrowserButton> activeButtons = new List<EntityBrowserButton>();
    private Queue<EntityBrowserButton> pooledButtons = new Queue<EntityBrowserButton>();

    private void Awake()
    {
        Main = this;
    }

    private void Start()
    {
        RenderFolder(EntityDatabase.main.RootFolder);
    }

    private void ResetWindow()
    {
        foreach (var button in activeButtons)
        {
            button.rectTransform.SetParent(poolParent);
            pooledButtons.Enqueue(button);
        }
        activeButtons.Clear();
        for (int i = 0; i < directoryButtonsParent.childCount; i++)
        {
            Destroy(directoryButtonsParent.GetChild(i).gameObject);
        }
    }

    public void RenderFolder(string folderPath)
    {
        if (EntityDatabase.main.TryGetFolder(folderPath, out var folder))
        {
            RenderFolder(folder);
        }
        else if (string.IsNullOrEmpty(folderPath))
        {
            RenderFolder(EntityDatabase.main.RootFolder);
        }
        else
        {
            ErrorMessage.AddMessage($"Failed to load folder at path '{folderPath}'!");
        }
    }

    public void RenderFolder(EntityBrowserFolder folder)
    {
        lastActiveFolderPath = activeFolderPath;
        visitedFolderHistory.Add(lastActiveFolderPath);

        ResetWindow();

        // Draw subentries

        foreach (var item in folder.Subentries)
        {
            RenderBrowserEntry(item);
        }

        // Draw directory buttons
        
        var directoriesList = folder.GetParentFolders();
        directoriesList.Add(folder);
        if (!directoriesList.Contains(EntityDatabase.main.RootFolder)) directoriesList.Insert(0, EntityDatabase.main.RootFolder);

        for (int i = 0; i < directoriesList.Count; i++)
        {
            var button = Instantiate(directoryButtonPrefab);
            button.GetComponent<RectTransform>().SetParent(directoryButtonsParent);
            button.GetComponent<EntityBrowserButton>().SetBrowserEntry(directoriesList[i]);
            if (i < directoriesList.Count - 1)
            {
                Instantiate(directorySeparatorPrefab).GetComponent<RectTransform>().SetParent(directoryButtonsParent);
            }
        }

        activeFolderPath = folder.Path;
        showingSearchResults = false;
        searchBar.text = null;

        StopAllCoroutines();
        StartCoroutine(GenerateSpritesForPage());
    }

    public void OnUpdateFilterInput()
    {
        if (string.IsNullOrEmpty(searchBar.text))
        {
            RenderFolder(EntityDatabase.main.RootFolder);
            return;
        }

        Filter(searchBar.text);

        showingSearchResults = true;
    }

    public void Filter(string searchString)
    {
        ResetWindow();

        foreach (var folder in EntityDatabase.main.AllFolders)
        {
            if (FilterFolder(folder, searchString))
            {
                RenderBrowserEntry(folder);
            }
        }

        foreach (var entity in EntityDatabase.main.AllEntitiesInBrowser)
        {
            if (FilterEntity(entity, searchString))
            {
                RenderBrowserEntry(entity);
            }
        }
        
        StopAllCoroutines();
        StartCoroutine(GenerateSpritesForPage());
    }

    // Does anyone here know regex? On top of cases, we could ignore spaces and underscores too

    private bool FilterFolder(EntityBrowserFolder folder, string searchString)
    {
        return folder.Name.IndexOf(searchString, System.StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private bool FilterEntity(EntityBrowserEntity entity, string searchString)
    {
        return entity.EntityData.ClassId.IndexOf(searchString, System.StringComparison.OrdinalIgnoreCase) >= 0 
               || entity.Name.IndexOf(searchString, System.StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private void RenderBrowserEntry(EntityBrowserEntryBase entry)
    {
        EntityBrowserButton button;
        if (pooledButtons.Count > 0)
        {
            button = pooledButtons.Dequeue();
        }
        else
        {
            button = Instantiate(buttonPrefab).GetComponent<EntityBrowserButton>();
        }
        button.rectTransform.SetParent(contentParent);
        button.SetBrowserEntry(entry);
        entry.OnConstructButton(button.gameObject);
        activeButtons.Add(button);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse4))
        {
            GoUnback();
        }
        if (Input.GetKeyDown(KeyCode.Mouse3))
        {
            GoBack();
        }
    }

    public void GoBack()
    {
        if (showingSearchResults)
        {
            RenderFolder(activeFolderPath);
            return;
        }
        string folderToReturnTo;
        if (visitedFolderHistory.Count <= 0) folderToReturnTo = null;
        else folderToReturnTo = visitedFolderHistory[visitedFolderHistory.Count - 1];
        if (string.IsNullOrEmpty(folderToReturnTo))
            RenderFolder(EntityDatabase.main.RootFolder);
        else
            RenderFolder(folderToReturnTo);
        visitedFolderHistory.RemoveAt(visitedFolderHistory.Count - 1);
        if (visitedFolderHistory.Count > 0)
        {
            visitedFolderHistory.RemoveAt(visitedFolderHistory.Count - 1);
        }
    }

    public void GoUnback() // tf is that button called?
    {
        RenderFolder(lastActiveFolderPath);
    }

    private IEnumerator GenerateSpritesForPage()
    {
        foreach (var button in activeButtons)
        {
            var entry = button.GetBrowserEntry();
            if (entry is not EntityBrowserEntity entity) continue;
            if (entity.EntityData.ClassId == "PrecursorCacheBaseModel") continue;
            var classId = entity.EntityData.ClassId;
            if (IconGenerator.HasIcon(classId)) continue;
            var task = PrefabDatabase.GetPrefabAsync(classId);
            yield return task;
            if (!task.TryGetPrefab(out var prefab)) continue;
            var output = new IconGenerator.IconOutput();
            yield return IconGenerator.GenerateIcon(prefab, classId, output);
            button.image.sprite = output.Sprite;
            // yield return null;
        }
    }
}