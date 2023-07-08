using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlueprintSearchBar;

internal class SearchBarBehaviour : MonoBehaviour
{
    public static SearchBarBehaviour main;
    private TMP_InputField input;
    private Transform buttonsParent;

    private void Awake()
    {
        buttonsParent = transform.parent.Find("ScrollView/Viewport/ScrollCanvas");
        input = GetComponent<TMP_InputField>();
        input.onValueChanged.AddListener(OnChange);
        main = this;
    }

    private void OnChange(string str)
    {
        Refresh();
    }

    public void Refresh()
    {
        var searchString = input.text.ToLower();
        foreach (Transform child in buttonsParent)
        {
            bool anyShown = false;
            if (child.gameObject.name != "CategoryCanvas")
                continue;
            foreach (Transform entry in child)
            {
                var text = entry.Find("Title").GetComponent<TextMeshProUGUI>().text;
                if (text != null)
                {
                    var shown = text.ToLower().Contains(searchString);
                    entry.gameObject.SetActive(shown);
                    if (!anyShown && shown) anyShown = true;
                }
            }
            buttonsParent.transform.GetChild(child.GetSiblingIndex() - 1).gameObject.SetActive(anyShown);
        }
    }
}
