using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AuroraDecalFixMod.Data;
using UnityEngine;
using UWE;

namespace AuroraDecalFixMod.DevTools;

public static class PrefabDecalScraper
{
    public static IEnumerator ScrapeGameForPrefabs((string,Func<String, bool>)[] validLocations, string outputFilePath)
    {
        List<PrefabDecalData> allPrefabsData = new List<PrefabDecalData>();
        var files = PrefabDatabase.prefabFiles;
        foreach (var prefabData in files)
        {
            string prefabPath = prefabData.Value;
            bool isInValidFolder = false;
            foreach (var validPrefix in validLocations)
            {
                if (prefabPath.StartsWith(validPrefix.Item1, StringComparison.OrdinalIgnoreCase))
                {
                    isInValidFolder = validPrefix.Item2.Invoke(prefabPath);
                }
            }

            if (!isInValidFolder)
                continue;

            IPrefabRequest prefabTask = PrefabDatabase.GetPrefabAsync(prefabData.Key);
            yield return prefabTask;
            if (!prefabTask.TryGetPrefab(out GameObject prefab))
            {
                Plugin.Logger.LogWarning(string.Format("Prefab '{0}' seems to be invalid!", prefabData.Value));
                continue;
            }

            List<RendererDecalsData> rendererDecalsData = new List<RendererDecalsData>();
            Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>(true);
            foreach (var renderer in renderers)
            {
                string path = GetLocalPath(prefab.transform, renderer.transform);
                if (string.IsNullOrEmpty(path))
                {
                    Plugin.Logger.LogWarning("GetLocalPath method did not work as expected!");
                    continue;
                }

                var materialIndices = new List<int>();
                for (int i = 0; i < renderer.sharedMaterials.Length; i++)
                {
                    if (IsMaterialDecal(renderer.sharedMaterials[i]))
                    {
                        materialIndices.Add(i);
                    }
                }

                if (materialIndices.Count > 0)
                {
                    rendererDecalsData.Add(new RendererDecalsData
                    {
                        RendererPath = path,
                        DecalMaterialIndices = materialIndices.ToArray()
                    });   
                }
            }

            if (rendererDecalsData.Count > 0)
            {
                allPrefabsData.Add(new PrefabDecalData
                {
                    ClassId = prefabData.Key,
                    Renderers = rendererDecalsData.ToArray()
                });
            }
        }

        string serialized = Newtonsoft.Json.JsonConvert.SerializeObject(new AllDecalData()
        {
            Prefabs = allPrefabsData.ToArray()
        });
        File.WriteAllText(outputFilePath, serialized);
    }

    private static string GetLocalPath(Transform ancestor, Transform descendant)
    {
        if (ancestor == null || descendant == null)
            return null;

        Transform current = descendant;
        string path = current.name;

        while (current.parent != null && current.parent != ancestor)
        {
            current = current.parent;
            path = current.name + "/" + path;
        }

        if (current.parent == ancestor)
            return path;

        return null;
    }

    private static bool IsMaterialDecal(Material material)
    {
        return material.name.Contains("exterrior_hull_dirt_decals") ||
               material.name.StartsWith("starship_exploded_interrior_wallmods_01_01_damage_03") ||
               material.name.StartsWith("starship_exploded_interrior_wallmods_01_01_damage_02");
    }
}