using System.Collections;
using ModStructureHelperPlugin.EntityHandling.Icons;
using ModStructureHelperPlugin.Mono;
using ModStructureHelperPlugin.StructureHandling;
using UnityEngine;
using UWE;

namespace ModStructureHelperPlugin.EntityHandling;

public class EntityBrowserEntity : EntityBrowserEntryBase
{
    public EntityData EntityData { get; }
    public override string Name => Plugin.ModConfig.BrowserUsesDebugNames ? EntityData.Name : EntityData.TranslatedName;

    public override EntityIcon Icon => IconGenerator.TryGetIcon(EntityData.ClassId, out var sprite)
        ? sprite
        : EntityDatabase.main.DefaultEntityIcon;

    public override string GetTooltip()
    {
        string str;
        
        if (EntityData.ClassId.Equals(EntityData.Name))
        {
            str = EntityData.Name;
        }
        else
        {
            str = $"{EntityData.Name}\n<size=12>{EntityData.ClassId}</size>";
        }

        str += $"\n({EntityData.SourceName})";
        return str;
    }

    public EntityBrowserEntity(string path, EntityData entity) : base(path)
    {
        EntityData = entity;
    }

    public override void OnInteract()
    {
        if (StructureInstance.Main == null)
        {
            ErrorMessage.AddMessage("You must first create or load a structure!");
            return;
        }

        CoroutineHost.StartCoroutine(SpawnEntityIntoStructure(StructureInstance.Main));
        // ErrorMessage.AddMessage($"Failed to spawn entity by Class ID '{EntityData.ClassId}' (behavior not implemented yet!");
    }

    private IEnumerator SpawnEntityIntoStructure(StructureInstance structure)
    {
        var task = PrefabDatabase.GetPrefabAsync(EntityData.ClassId);
        yield return task;
        if (!task.TryGetPrefab(out var prefab))
        {
            ErrorMessage.AddMessage($"Failed to spawn entity by Class ID '{EntityData.ClassId}' (prefab not found!)");
            yield break;
        }

        if (structure == null)
        {
            ErrorMessage.AddMessage("Error: the structure was deselected.");
            yield break;
        }

        var spawned = structure.SpawnPrefabIntoStructure(prefab, true);
        var maxRayDist = 10f;
        var spawnPos = MainCamera.camera.transform.position + MainCamera.camera.transform.forward * maxRayDist;
        var spawnUp = Vector3.up;
        if (Physics.Raycast(MainCamera.camera.transform.position + MainCamera.camera.transform.forward,
                MainCamera.camera.transform.forward, out var hit, maxRayDist, -1, QueryTriggerInteraction.Ignore))
        {
            spawnPos = hit.point;
            spawnUp = hit.normal;
        }

        spawned.transform.position = spawnPos;
        spawned.transform.up = spawnUp;
    }

    public override void OnConstructButton(GameObject button)
    {
        // enable the paint button
        button.transform.GetChild(1).gameObject.SetActive(true);
    }
}