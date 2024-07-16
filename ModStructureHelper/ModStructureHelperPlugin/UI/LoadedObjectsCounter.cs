using ModStructureHelperPlugin.StructureHandling;
using TMPro;
using UnityEngine;

namespace ModStructureHelperPlugin.UI;

public class LoadedObjectsCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    
    private void Update()
    {
        if (StructureInstance.Main == null)
        {
            text.text = "N/A";
            return;
        }

        text.text = $"{StructureInstance.Main.GetLoadedEntityCount()}/{StructureInstance.Main.GetTotalEntityCount()}";
    }
}