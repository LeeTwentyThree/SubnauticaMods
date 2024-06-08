using ModStructureFormat;
using UnityEngine;

namespace ModStructureHelperPlugin;

public class StructureInstance : MonoBehaviour
{
    public static StructureInstance main;
    
    public Structure data;
    public string path;

    public static void CreateNewInstance(Structure data, string path)
    {
        if (main != null)
        {
            ErrorMessage.AddMessage("An existing structure instance already exists!");
            return;
        }

        var instance = new GameObject("StructureInstance").AddComponent<StructureInstance>();
        instance.data = data;
        instance.path = path;
    }

    public static void TrySave()
    {
        ErrorMessage.AddMessage("Saving current structure...");
        if (main == null)
        {
            ErrorMessage.AddMessage("There is nothing to save!");
            return;
        }
        main.Save();
        ErrorMessage.AddMessage($"Successfully saved to path '{main.path}.'");
    }
    
    private void Awake()
    {
        main = this;
    }

    private void Save()
    {
        ErrorMessage.AddMessage("Save logic not yet implemented...");
    }
}