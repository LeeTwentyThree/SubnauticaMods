using System;
using System.Collections.Generic;
using System.Linq;
using ModStructureFormat;
using UnityEngine;

namespace ModStructureHelperPlugin;

public class StructureInstance : MonoBehaviour
{
    public static StructureInstance Main;
    
    public Structure data;
    public string path;

    private List<ManagedEntity> _managedEntities = new List<ManagedEntity>();

    public static void CreateNewInstance(Structure data, string path)
    {
        if (Main != null)
        {
            ErrorMessage.AddMessage("An existing structure instance already exists!");
            return;
        }

        var instance = new GameObject("StructureInstance").AddComponent<StructureInstance>();
        instance.data = data;
        instance.path = path;

        instance._managedEntities = new List<ManagedEntity>(data.Entities.Select(e => new ManagedEntity(e)));
        
        instance.TryGrabManagedEntities();
    }

    public static void TrySave()
    {
        ErrorMessage.AddMessage("Saving current structure...");
        if (Main == null)
        {
            ErrorMessage.AddMessage("There is nothing to save!");
            return;
        }
        Main.Save();
        ErrorMessage.AddMessage($"Successfully saved to path '{Main.path}.'");
    }

    public GameObject SpawnPrefabIntoStructure(GameObject prefab)
    {
        var obj = Instantiate(prefab);
        obj.SetActive(true);
        var instance = obj.AddComponent<EntityInstance>();
        _managedEntities.Add(new ManagedEntity(instance));
        return obj;
    }
    
    public Structure GetCurrentStructureData()
    {
        var savedEntities = new Entity[_managedEntities.Count];
        for (var i = 0; i < _managedEntities.Count; i++)
        {
            if (_managedEntities[i].EntityInstance != null)
            {
                savedEntities[i] = _managedEntities[i].EntityInstance.GetEntityDataStruct();
            }
            else
            {
                savedEntities[i] = _managedEntities[i].EntityData;
            }
        }
        return new Structure(savedEntities);
    }
    
    private void Awake()
    {
        Main = this;
    }

    private void Save()
    {
        GetCurrentStructureData().SaveToFile(path);
        ErrorMessage.AddMessage("Save logic not yet implemented... well it might be now lol");
    }

    private void TryGrabManagedEntities()
    {
        // print # of not-found entities and name them
    }
}