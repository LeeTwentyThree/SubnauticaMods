using ModStructureFormat;
using TMPro;
using UnityEngine;

namespace ModStructureHelperPlugin.UI.Buttons;

public class EditStructureButton : MonoBehaviour
{
    public Structure structure;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI pathText;
    
    public void SetStructure(Structure structure, string name, string path)
    {
        this.structure = structure;
        nameText.text = name;
        pathText.text = path;
    }

    public void OnButtonPressed()
    {
        ErrorMessage.AddMessage("Loading structure!");
    }
}