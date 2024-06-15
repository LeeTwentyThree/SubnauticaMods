using System.Collections;
using System.Collections.Generic;
using ModStructureFormat;
using UnityEngine;

namespace ModStructureHelperPlugin;

public static class EntityUtility
{
    public static IEnumerator SpawnEntitiesFromStructure(Structure structure)
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

            var structureInstance = StructureInstance.Main;
            if (structureInstance == null)
            {
                ErrorMessage.AddMessage("Structure instance was unloaded! Canceling structure spawning.");
                yield break;
            }

            if (structureInstance.IsEntityLoadedIntoWorld(data.id))
            {
                continue;
            }

            if (prefab == null)
            {
                ErrorMessage.AddMessage($"Prefab for Class Id '{data.classId}' is null!");
                continue;
            }
            var obj = Object.Instantiate(prefab);
            obj.transform.position = data.position.ToVector3();
            obj.transform.rotation = data.rotation.ToQuaternion();
            obj.transform.localScale = data.scale.ToVector3();
            var prefabIdentifier = obj.GetComponent<PrefabIdentifier>();
            if (prefabIdentifier == null)
            {
                Plugin.Logger.LogError($"Somehow, prefab '{prefab.name}' of Class Id is {data.classId} is lacking a PrefabIdentifier component!");
                continue;
            }
            prefabIdentifier.GetComponent<UniqueIdentifier>().Id = data.id;
            structureInstance.AttemptEntityRegistration(prefabIdentifier);
        }
    }
}