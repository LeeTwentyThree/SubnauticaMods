﻿using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using UnityEngine;
using System.Collections;
using Nautilus.Utility;
using TheRedPlague.Mono;

namespace TheRedPlague.PrefabFiles;

public static class InfectionDome
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("InfectionDome");

    public static void Register()
    {
        var infectionDome = new CustomPrefab(Info);
        infectionDome.SetGameObject(GetPrefab);
        infectionDome.SetSpawns(new SpawnLocation(Vector3.zero, Vector3.zero, Vector3.one * 1000));
        infectionDome.Register();
    }
    
    private static IEnumerator GetPrefab(IOut<GameObject> prefab)
    {
        var obj = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("InfectionDome"));
        MaterialUtils.ApplySNShaders(obj);
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType,
            LargeWorldEntity.CellLevel.Global);
        var renderer = obj.GetComponentInChildren<Renderer>();
        var material = new Material(MaterialUtils.ForceFieldMaterial);
        material.SetColor("_Color", new Color(1, 0, 0, 0.95f));
        material.SetVector("_MainTex2_Speed", new Vector4(-0.01f, 0.1f, 0, 0));
        var materials = renderer.materials;
        materials[2] = material;
        renderer.materials = materials;

        var solarPanelRequest = CraftData.GetPrefabForTechTypeAsync(TechType.SolarPanel);
        yield return solarPanelRequest;
        var solarPanelPrefab = solarPanelRequest.GetResult();

        var linePrefab = Object.Instantiate(solarPanelPrefab.GetComponent<PowerFX>().vfxPrefab, obj.transform);
        linePrefab.SetActive(false);
        var line = linePrefab.GetComponent<LineRenderer>();
        var newMaterial = new Material(line.material);
        newMaterial.color = new Color(4, 0, 0);
        line.material = newMaterial;
        line.widthMultiplier = 1;

        var lightning = obj.AddComponent<InfectionDomeController>();
        lightning.linePrefab = linePrefab;

        prefab.Set(obj);
    }
}