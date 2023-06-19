using TMPro;
using UnityEngine.UI;

namespace CreatureMorphs.Mono.UI;

internal class MorphMenu : MonoBehaviour
{
    public static MorphMenu main;

    public RectTransform buttonsParent;

    private GameObject buttonPrefab;

    public static MorphMenu CreateInstance()
    {
        main = new GameObject("MorphMenu").AddComponent<MorphMenu>();
        var canvas = Instantiate(Plugin.bundle.LoadAsset<GameObject>("MorphCanvas"));
        canvas.transform.parent = main.transform;
        canvas.transform.localPosition = Vector3.zero;
        canvas.transform.localRotation = Quaternion.identity;

        main.buttonsParent = canvas.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();

        main.RegenerateButtons();

        return main;
    }

    private void RegenerateButtons()
    {
        if (buttonPrefab == null)
        {
            buttonPrefab = transform.GetChild(0).Find("ButtonsMask/MorphButtonReference").gameObject;
        }
        foreach (Transform child in buttonsParent.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var morph in MorphDatabase.GetAllMorphTypes())
        {
            if (KnownMorphs.IsUnlocked(morph))
            {
                AddButton(morph);
            }
        }
    }

    private void AddButton(MorphDatabase.Entry entry)
    {
        var spawned = Instantiate(buttonPrefab, buttonsParent.transform);
        spawned.transform.GetChild(0).GetComponent<Image>().sprite = GetSpriteForCreature(entry.MainTechType);
        spawned.GetComponentInChildren<TextMeshProUGUI>().text = Language.main.Get(entry.MainTechType);
        spawned.AddComponent<MorphMenuButton>().entry = entry;

        spawned.SetActive(true);
    }

    private static Sprite GetSpriteForCreature(TechType tt)
    {
        return Plugin.bundle.LoadAsset<Sprite>(tt.AsString(false));
    }
}