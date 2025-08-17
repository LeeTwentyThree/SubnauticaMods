using System.Collections;
using AuroraDecalFixMod.Data;
using Nautilus.Handlers;
using UnityEngine;
using UWE;

namespace AuroraDecalFixMod.PrefabModification;

public class AuroraPrefabModifier
{
    private static readonly int ZOffset = Shader.PropertyToID("_ZOffset");
    
    private AllDecalData Data { get; }
    
    public AuroraPrefabModifier(AllDecalData data)
    {
        Data = data;
    }

    public IEnumerator ModifyAll(WaitScreenHandler.WaitScreenTask task)
    {
        int totalPrefabs = Data.Prefabs.Length;
        int c = 0;
        
        foreach (var prefab in Data.Prefabs)
        {
            task.Status = FormatCurrentTaskStatus(c, totalPrefabs);
            var prefabTask = PrefabDatabase.GetPrefabAsync(prefab.ClassId);
            yield return prefabTask;
            if (!prefabTask.TryGetPrefab(out GameObject toModify))
            {
                Plugin.Logger.LogWarning($"Prefab '{prefab.ClassId}' could not be found. Skipping modification.");
            }
            else
            {
                foreach (var rendererData in prefab.Renderers)
                {
                    bool isTRoom = rendererData.RendererPath == "starship_exploded_interior_T_room";
                    Transform rendererChild = toModify.transform.Find(rendererData.RendererPath);
                    if (rendererChild == null)
                    {
                        Plugin.Logger.LogWarning($"Child not found at path '{rendererData.RendererPath}' for prefab '{prefab.ClassId}'");
                        continue;
                    }
                    var renderer = rendererChild.GetComponent<Renderer>();
                    var materials = renderer.sharedMaterials;
                    foreach (var index in rendererData.DecalMaterialIndices)
                    {
                        var material = new Material(materials[index]);
                        material.EnableKeyword("MARMO_ALPHA_CLIP");
                        material.SetFloat(ZOffset, isTRoom && index == 36 ? 100000 : 0);
                        materials[index] = material;
                    }
                    renderer.sharedMaterials = materials;
                }
            }
            c++;
        }
    }

    private string FormatCurrentTaskStatus(int taskIndex, int totalTasks)
    {
        int percent = Mathf.RoundToInt(taskIndex / (float)totalTasks);
        return string.Format("Fixing decals ({0}%)", percent);
    }
}