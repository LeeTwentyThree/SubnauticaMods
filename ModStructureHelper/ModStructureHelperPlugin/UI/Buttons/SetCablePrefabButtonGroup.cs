using ModStructureHelperPlugin.CableGeneration;
using ModStructureHelperPlugin.Tools;
using UnityEngine;

namespace ModStructureHelperPlugin.UI.Buttons;

public class SetCablePrefabButtonGroup : MonoBehaviour
{
    [SerializeField] private CableGeneratorTool tool;
    [SerializeField] private SetCablePrefabButton[] buttons;
    [SerializeField] private CableLocation location;
    [SerializeField] private Sprite defaultImage;
    [SerializeField] private Sprite selectedImage;

    private void OnValidate()
    {
        buttons = GetComponentsInChildren<SetCablePrefabButton>();
    }

    private void Start()
    {
        buttons[0].OnButtonPressed();
    }

    public void SetCablePrefab(string classId, SetCablePrefabButton button)
    {
        tool.SetCablePrefab(location, classId);
        
        foreach (var b in buttons)
        {
            var selected = b == button;
            b.SetImageSprite(selected ? selectedImage : defaultImage);
        }
    }
}