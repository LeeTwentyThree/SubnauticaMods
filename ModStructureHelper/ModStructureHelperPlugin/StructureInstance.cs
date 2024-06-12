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

    public delegate void OnStructureInstanceUpdatedHandler(StructureInstance newInstance);

    public static event OnStructureInstanceUpdatedHandler OnStructureInstanceUpdated; 

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

        OnStructureInstanceUpdated?.Invoke(instance);
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
        var managedEntity = new ManagedEntity(instance);
        instance.ManagedEntity = managedEntity;
        _managedEntities.Add(managedEntity);
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

    public bool IsEntityPartOfStructure(string id)
    {
        return _managedEntities.Any(entity => entity.Id == id);
    }

    public Vector3 GetStructureCenterPosition()
    {
        var sumOfPositions = Vector3.zero;
        foreach (var entity in _managedEntities)
        {
            sumOfPositions += entity.Position;
        }

        var count = _managedEntities.Count;
        return new Vector3(sumOfPositions.x / count, sumOfPositions.y / count, sumOfPositions.z / count);
    }
    
    private void Awake()
    {
        Main = this;
    }

    private void OnDestroy()
    {
        OnStructureInstanceUpdated?.Invoke(null);
    }

    private void Save()
    {
        GetCurrentStructureData().SaveToFile(path);
    }

    private void TryGrabManagedEntities()
    {
        foreach (var identifier in UniqueIdentifier.AllIdentifiers)
        {
            if (identifier is PrefabIdentifier prefabIdentifier)
            {
                AttemptEntityRegistration(prefabIdentifier);
            }
        }

        var unmanagedEntitiesClassIds = new List<string>();
        foreach (var managedEntity in _managedEntities)
        {
            if (managedEntity.EntityInstance == null)
            {
                unmanagedEntitiesClassIds.Add(managedEntity.ClassId);
            }
        }

        // print # of not-found entities and name them

        if (unmanagedEntitiesClassIds.Count == 0) return;
        ErrorMessage.AddMessage($"{unmanagedEntitiesClassIds.Count} entities are still not loaded. As you get closer, these will likely load in.\nCurrently unmanaged entities:");
        var printed = 0;
        foreach (var classId in unmanagedEntitiesClassIds)
        {
            if (printed >= 10)
            {
                ErrorMessage.AddMessage($"And {unmanagedEntitiesClassIds.Count - printed} more...");
                break;
            }
            ErrorMessage.AddMessage("- " + classId);
            printed++;
        }
    }

    public void AttemptEntityRegistration(PrefabIdentifier prefabIdentifier)
    {
        foreach (var managedEntity in _managedEntities)
        {
            if (managedEntity.Id != prefabIdentifier.Id) continue;
            var entityInstance = prefabIdentifier.gameObject.GetComponent<EntityInstance>();
            if (entityInstance != null)
            {
                Plugin.Logger.LogWarning($"Object '{prefabIdentifier.gameObject}' was already an Entity Instance!");
                return;
            }
            entityInstance = prefabIdentifier.gameObject.AddComponent<EntityInstance>();
            managedEntity.SetEntityInstance(entityInstance);
        }
    }
}