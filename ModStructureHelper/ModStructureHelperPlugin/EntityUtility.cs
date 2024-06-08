using System.Collections;
using System.Collections.Generic;
using ModStructureFormat;
using UnityEngine;

namespace ModStructureHelperPlugin;

public static class EntityUtility
{
    public static IEnumerator SpawnEntitiesFromStructure(Structure structure, List<EntityInstance> resultsList)
    {
        if (!structure.IsSorted)
            structure.SortByPriority();
        var loadedPrefabs = new Dictionary<string, GameObject>();
        foreach (var data in structure.Entities)
        {
            if (!loadedPrefabs.TryGetValue(data.classId, out var prefab))
            {
                var task = UWE.PrefabDatabase.GetPrefabAsync(data.classId);
                yield return task;
                if (!task.TryGetPrefab(out prefab)) ErrorMessage.AddMessage($"Failed to load prefab for prefab of Class ID {data.classId}");
                loadedPrefabs.Add(data.classId, prefab);
            }

            var obj = Object.Instantiate(prefab);
            obj.transform.position = data.position;
            obj.transform.eulerAngles = data.eulerAngles;
            obj.transform.localScale = data.scale;

            var entityInstance = obj.AddComponent<EntityInstance>();
            entityInstance.priority = data.priority;

            resultsList.Add(entityInstance);
        }
    }
}