namespace CreatureMorphs.Mono;

internal class MorphMenu : MonoBehaviour
{
    public static MorphMenu main;

    public RectTransform buttonsParent;

    private GameObject buttonPrefab;

    public static MorphMenu CreateInstance()
    {
        main = new GameObject().AddComponent<MorphMenu>();
        var canvas = Instantiate(Plugin.bundle.LoadAsset<GameObject>("MorphCanvas"));
        canvas.transform.parent = main.transform;
        canvas.transform.localPosition = Vector3.zero;
        canvas.transform.localRotation = Quaternion.identity;

        main.buttonsParent = canvas.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();

        return main;
    }

    private void RegenerateButtons()
    {
        if (buttonPrefab == null)
        {
            buttonPrefab = Plugin.bundle.LoadAsset<GameObject>("MorphButton");
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
        var sprite = GetSpriteForCreature(entry.MainTechType);
        Instantiate(buttonPrefab, buttonsParent.transform);
    }

    private static Sprite GetSpriteForCreature(TechType tt)
    {
        return Plugin.bundle.LoadAsset<Sprite>(tt.AsString(false));
    }
}