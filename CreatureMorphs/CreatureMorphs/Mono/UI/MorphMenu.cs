using TMPro;
using UnityEngine.UI;

namespace CreatureMorphs.Mono.UI;

internal class MorphMenu : MonoBehaviour
{
    public static MorphMenu main;

    public RectTransform buttonsParent;

    private GameObject buttonPrefab;

    private static int _selectedButton;

    private int ButtonsCount => buttonsParent.childCount;

    public static MorphMenu CreateInstance()
    {
        main = new GameObject("MorphMenu").AddComponent<MorphMenu>();
        var canvas = Instantiate(Plugin.bundle.LoadAsset<GameObject>("MorphCanvas"));
        canvas.transform.parent = main.transform;
        canvas.transform.localPosition = Vector3.zero;
        canvas.transform.localRotation = Quaternion.identity;

        main.buttonsParent = canvas.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();

        FontUtils.SetFontInChildren(main.gameObject, FontUtils.Aller_Rg);

        main.gameObject.SearchChild("Controls").GetComponent<TextMeshProUGUI>().text =
            $"Navigate: {Helpers.GetInputString(GameInput.Button.UILeft)} / {Helpers.GetInputString(GameInput.Button.UIRight)}\n"
            + $"Select: {Helpers.GetInputString(GameInput.Button.LeftHand)}";


        main.RegenerateButtons();

        return main;
    }

    private void RegenerateButtons()
    {
        if (buttonPrefab == null)
        {
            buttonPrefab = transform.GetChild(0).Find("ButtonsMask/MorphButtonReference").gameObject;
            FontUtils.SetFontInChildren(buttonPrefab, FontUtils.Aller_W_Bd);
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
        UpdateHoveredButton();
    }

    private void Update()
    {
        HandleInput();
    }

    private void UpdateHoveredButton()
    {
        if (_selectedButton < 0) _selectedButton = ButtonsCount - 1;
        if (_selectedButton >= ButtonsCount) _selectedButton = 0;
        var buttons = buttonsParent.GetComponentsInChildren<MorphMenuButton>();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetSpriteHovered(i == _selectedButton);
        }
    }

    private void HandleInput()
    {
        if (GameInput.GetButtonDown(GameInput.Button.UILeft))
        {
            _selectedButton--;
        }
        if (GameInput.GetButtonDown(GameInput.Button.UIRight))
        {
            _selectedButton++;
        }
        UpdateHoveredButton();
        if (GameInput.GetButtonDown(GameInput.Button.LeftHand) && ButtonsCount > 0)
        {
            buttonsParent.transform.GetChild(_selectedButton).GetComponent<MorphMenuButton>().OnSelect();
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