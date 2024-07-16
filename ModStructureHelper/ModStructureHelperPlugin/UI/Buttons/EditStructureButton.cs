using ModStructureFormat;
using ModStructureHelperPlugin.StructureHandling;
using TMPro;
using UnityEngine;

namespace ModStructureHelperPlugin.UI.Buttons;

public class EditStructureButton : MonoBehaviour
{
    public Structure structure;
    public string path;

    [SerializeField] private StructureHelperUI ui;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI pathText;
    
    public void SetStructure(Structure structure, string name, string path)
    {
        this.structure = structure;
        this.path = path;
        nameText.text = name;
        pathText.text = path;
    }

    public void OnButtonPressed()
    {
        ErrorMessage.AddMessage("Loading structure!");
        StructureInstance.CreateNewInstance(structure, path);
        ui.SetMenuActive(MenuType.Editing);
    }
}