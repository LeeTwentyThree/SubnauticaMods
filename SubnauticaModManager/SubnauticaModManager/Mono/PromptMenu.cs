using System;
using UnityEngine.Events;

namespace SubnauticaModManager.Mono;

internal class PromptMenu : MonoBehaviour
{
    private TextMeshProUGUI text;
    private Transform buttonArea;
    private GameObject promptButtonReference;
    private Sprite warningSprite;

    private void Awake()
    {
        text = transform.Find("TextArea").GetComponent<TextMeshProUGUI>();
        buttonArea = transform.Find("ButtonArea");
        promptButtonReference = transform.Find("PromptButtonReference").gameObject;
        warningSprite = Plugin.assetBundle.LoadAsset<Sprite>("Panel-Warning");
    }

    public void Ask(string text, params PromptChoice[] choices)
    {
        gameObject.SetActive(true); // must do this first so the awake method has been called!

        this.text.text = text;
        foreach (Transform child in buttonArea)
        {
            Destroy(child.gameObject);
        }
        foreach (var choice in choices)
        {
            CreateButton(choice);
        }
    }

    private void CreateButton(PromptChoice choice)
    {
        var spawned = Instantiate(promptButtonReference);
        spawned.SetActive(true);
        spawned.GetComponent<RectTransform>().SetParent(buttonArea, false);
        spawned.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;
        var button = spawned.GetComponent<Button>();
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(OnAnyChoiceSelected);
        if (choice.action != null)
        {
            button.onClick.AddListener(new UnityAction(choice.action));
        }
        if (choice.dangerous)
        {
            button.image.sprite = warningSprite;
        }
    }

    private void OnAnyChoiceSelected()
    {
        gameObject.SetActive(false);
        SoundUtils.PlaySound(UISound.Tweak);
    }
}
public struct PromptChoice
{
    public string text;
    public Action action;
    public bool dangerous;

    public PromptChoice(string text, bool dangerous, Action action)
    {
        this.text = text;
        this.action = action;
        this.dangerous = dangerous;
    }

    public PromptChoice(string text, Action action)
    {
        this.text = text;
        this.action = action;
        dangerous = false;
    }

    public PromptChoice(string text)
    {
        this.text = text;
        action = null;
        dangerous = false;
    }
}