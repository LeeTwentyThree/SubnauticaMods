using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace ModStructureHelperPlugin.UI;

public class PromptHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Transform buttonArea;
    [SerializeField] private GameObject promptButtonReference;

    public void Ask(string text, params PromptChoice[] choices)
    {
        gameObject.SetActive(true); // must do this first so the awake method has been called!

        this.text.text = text;
        foreach (Transform child in buttonArea)
        {
            Destroy(child.gameObject);
        }

        if (choices.Length == 0)
        {
            CreateButton(new PromptChoice("Okay"));
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
    }

    private void OnAnyChoiceSelected()
    {
        gameObject.SetActive(false);
    }
}
public struct PromptChoice
{
    public string text;
    public Action action;

    public PromptChoice(string text, Action action)
    {
        this.text = text;
        this.action = action;
    }


    public PromptChoice(string text)
    {
        this.text = text;
        action = null;
    }
}