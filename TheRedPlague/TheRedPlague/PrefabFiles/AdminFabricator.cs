using System.Collections;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Handlers;
using UnityEngine;

namespace TheRedPlague.PrefabFiles;

public static class AdminFabricator
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("AdminFabricator");
    public static CraftTree.Type AdminCraftTree { get; private set; }

    public static void Register()
    {
        var customPrefab = new CustomPrefab(Info);
        var template = new CloneTemplate(Info, TechType.Fabricator);
        template.ModifyPrefabAsync = ModifyPrefabAsync;
        customPrefab.SetGameObject(template);
        customPrefab.CreateFabricator(out var craftTree);
        AdminCraftTree = craftTree;
        customPrefab.Register();
    }

    private static IEnumerator ModifyPrefabAsync(GameObject prefab)
    {
        foreach (var renderer in prefab.GetComponentsInChildren<Renderer>(true))
        {
            if (renderer is ParticleSystemRenderer) continue;
            var material = renderer.material;
            material.mainTexture = Plugin.AssetBundle.LoadAsset<Texture2D>("admin_fabricator_diffuse");
            material.SetTexture("_SpecTex", Plugin.AssetBundle.LoadAsset<Texture2D>("admin_fabricator_spec"));
            material.SetTexture("_Illum", Plugin.AssetBundle.LoadAsset<Texture2D>("adminfabricator_illum"));
        }
        
        Object.DestroyImmediate(prefab.GetComponent<Constructable>());
        Object.DestroyImmediate(prefab.GetComponent<ConstructableBounds>());
        Object.DestroyImmediate(prefab.GetComponent<PreventDeconstruction>());

        var fabricator = prefab.GetComponent<Fabricator>();
        fabricator.craftTree = AdminCraftTree;

        var solarPanelTask = CraftData.GetPrefabForTechTypeAsync(TechType.SolarPanel);
        yield return solarPanelTask;
        var solarPanel = solarPanelTask.GetResult();
        
        var relay = prefab.EnsureComponent<PowerRelay>();
        relay.powerSystemPreviewPrefab = solarPanel.GetComponent<PowerRelay>().powerSystemPreviewPrefab;
        fabricator.powerRelay = relay;
        
        var source = prefab.EnsureComponent<PowerSource>();
        source.maxPower = 15000;
        source.power = source.maxPower;

        relay.internalPowerSource = source;

        prefab.EnsureComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Global;

        yield break;
    }
}