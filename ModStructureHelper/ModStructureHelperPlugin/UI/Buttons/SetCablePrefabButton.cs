using UnityEngine;
using UnityEngine.UI;

namespace ModStructureHelperPlugin.UI.Buttons;

public class SetCablePrefabButton : MonoBehaviour
{
    [SerializeField] private SetCablePrefabButtonGroup group;
    [SerializeField] private Button button;
    [SerializeField] private string classId;

    private void OnValidate()
    {
        if (group == null)
            group = GetComponentInParent<SetCablePrefabButtonGroup>();
        button = GetComponent<Button>();
    }

    public void OnButtonPressed()
    {
        group.SetCablePrefab(classId, this);
    }

    public void SetImageSprite(Sprite sprite)
    {
        button.image.sprite = sprite;
    }
}