using System.Collections.Generic;
using ModStructureHelperPlugin.EntityHandling.Icons;
using Nautilus.Handlers;
using UnityEngine;

namespace ModStructureHelperPlugin.EntityHandling;

public class EntityDatabase : MonoBehaviour
{
    private const string BaseGameSourceName = "Subnautica";
    
    public static EntityDatabase main;
    [SerializeField]
    private Sprite defaultFolderSprite;
    [SerializeField]
    private Sprite defaultEntitySprite;
    
    public EntityIcon FolderIcon { get; private set; }
    public EntityIcon DefaultEntityIcon { get; private set; }

    // Key: class id
    private Dictionary<string, EntityData> entities = new Dictionary<string, EntityData>();
    
    // Key: path
    private Dictionary<string, EntityBrowserFolder> folders = new Dictionary<string, EntityBrowserFolder>();
    private Dictionary<string, EntityBrowserEntity> entityBrowserEntries = new Dictionary<string, EntityBrowserEntity>();

    public EntityBrowserFolder RootFolder { get; } = new EntityBrowserFolder(string.Empty);

    public IReadOnlyCollection<EntityBrowserEntity> AllEntitiesInBrowser => entityBrowserEntries.Values;
    public IReadOnlyCollection<EntityData> AllEntities => entities.Values;
    public IReadOnlyCollection<EntityBrowserFolder> AllFolders => folders.Values;

    private void Awake()
    {
        main = this;
        FolderIcon = new EntityIconBasic(defaultFolderSprite);
        DefaultEntityIcon = new EntityIconBasic(defaultEntitySprite);
        
        RegisterEntitiesFromPrefabInfo();
    }

    public void RegisterEntities(List<EntityData> newEntities)
    {
        foreach (var entity in newEntities)
        {
            entities.Add(entity.ClassId, entity);
        }
    }

    public void RegisterEntitiesFromPrefabInfo()
    {
        var data = UWE.PrefabDatabase.prefabFiles;
        foreach (var entity in data)
        {
            var path = entity.Value;
            bool hasModName = TryGetModName(entity.Key, out var modName);
            if (hasModName && !path.Contains("/")) path = GetPathWithModName(path, modName);
            var entityData = new EntityData(entity.Key, path,
                hasModName ? modName : BaseGameSourceName,
                GetTranslatedNameOrNull(entity.Key));
            entities.Add(entity.Key, entityData);
        }
        RebuildFolderStructure();
        // ErrorMessage.AddMessage($"Loaded {data.Count} entities from the prefab database into {folders.Count} folders!");
    }

    private static string GetTranslatedNameOrNull(string classId)
    {
        if (!CraftData.entClassTechTable.TryGetValue(classId, out var techType))
        {
            return null;
        }

        if (!Language.main.Contains(techType))
        {
            return null;
        }

        var translation = Language.main.Get(techType);

        if (string.IsNullOrEmpty(translation) || translation.Equals("None"))
        {
            return null;
        }

        return translation;
    }

    private static bool TryGetModName(string classId, out string modName)
    {
        var techTable = CraftData.entClassTechTable;
        if (!techTable.TryGetValue(classId, out var techType))
        {
            modName = null;
            return false;
        }

        if (EnumHandler.TryGetOwnerAssembly(techType, out var assembly))
        {
            modName = assembly.GetName().Name;
            return true;
        }
        
        modName = null;
        return false;
    }

    private static string GetPathWithModName(string path, string modName)
    {
        if (string.IsNullOrEmpty(modName))
            return path;
        
        var finalPath = $"{modName}/{path}";
        if (finalPath.EndsWith("Prefab")) return finalPath.Substring(0, finalPath.IndexOf("Prefab"));
        return finalPath;
    }

    public bool TryGetEntity(string entityClassId, out EntityData entity)
    {
        return entities.TryGetValue(entityClassId, out entity);
    }

    public bool TryGetFolder(string path, out EntityBrowserFolder folder)
    {
        return folders.TryGetValue(path, out folder);
    }

    private void RebuildFolderStructure()
    {
        folders = new Dictionary<string, EntityBrowserFolder>();
        entityBrowserEntries = new Dictionary<string, EntityBrowserEntity>();

        foreach (var entity in entities)
        {
            var path = entity.Value.Path;
            var containingFolderPath = PathUtils.GetParentDirectory(path);
            var folder = EnsureFolderExists(containingFolderPath);
            var browserEntry = new EntityBrowserEntity(path, entity.Value);
            entityBrowserEntries.Add(path, browserEntry);
            folder.Subentries.Add(browserEntry);
        }

        foreach (var folder in folders.Values)
        {
            folder.SortSubentries();
        }
    }

    private EntityBrowserFolder EnsureFolderExists(string path)
    {
        if (path == null) path = string.Empty;
        if (folders.TryGetValue(path, out var folder))
        {
            return folder;
        }
        var containingFolder = PathUtils.GetParentDirectory(path);
        EntityBrowserFolder parentFolder;
        if (string.IsNullOrEmpty(containingFolder))
        {
            parentFolder = RootFolder;
        }
        else if (!folders.TryGetValue(containingFolder, out parentFolder))
        {
            parentFolder = EnsureFolderExists(containingFolder);
        }
        var newFolder = new EntityBrowserFolder(path);
        if (parentFolder != null) parentFolder.Subentries.Add(newFolder);
        folders.Add(path, newFolder);
        return newFolder;
    }
}