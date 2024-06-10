using UnityEngine;

namespace ModStructureHelperPlugin.UI.Buttons;

public class GenericCloseButton : MonoBehaviour
{
    [SerializeField] private GameObject disableObject;

    public void OnButtonPressed() => disableObject.SetActive(false);
}