using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Utility;
using UnityEngine;
using UWE;

namespace PodshellLeviathan.Prefabs;

public static class NestPrefabs
{
    private static PrefabInfo RockInfo { get; } = PrefabInfo.WithTechType("PodshellNestRock");
    private static PrefabInfo SandInfo { get; } = PrefabInfo.WithTechType("PodshellNestSand");
    private static PrefabInfo LightInfo { get; } = PrefabInfo.WithTechType("PodshellNestLight");

    public static void Register()
    {
        var prefab = new CustomPrefab(RockInfo);
        prefab.SetGameObject(new CloneTemplate(RockInfo, "fa986d5a-0cf8-4c63-af9f-8c36acd5bea4")
        {
            ModifyPrefab = obj =>
            {
                obj.EnsureComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Far;
                foreach (var renderer in obj.GetComponentsInChildren<Renderer>(true))
                {
                    var material = renderer.material;
                    material.SetColor("_CapColor", new Color(0.48f, 0.52f, 1f));
                    material.SetColor("_CapSpecColor", new Color(0.57f, 0.666f, 1f));
                }
            }
        });
        prefab.Register();
        
        var sand = new CustomPrefab(SandInfo);
        sand.SetGameObject(() =>
        {
            var sandPrefab = Object.Instantiate(Plugin.Assets.LoadAsset<GameObject>("NestSandPrefab"));
            sandPrefab.SetActive(false);
            PrefabUtils.AddBasicComponents(sandPrefab, SandInfo.ClassID, SandInfo.TechType, LargeWorldEntity.CellLevel.Near);
            MaterialUtils.ApplySNShaders(sandPrefab, 3);
            return sandPrefab;
        });
        sand.Register();
        
        var light = new CustomPrefab(LightInfo);
        light.SetGameObject(() =>
        {
            var lightPrefab = new GameObject(LightInfo.ClassID);
            lightPrefab.SetActive(false);
            PrefabUtils.AddBasicComponents(lightPrefab, LightInfo.ClassID, LightInfo.TechType, LargeWorldEntity.CellLevel.Near);
            var pointLight = lightPrefab.AddComponent<Light>();
            pointLight.type = LightType.Point;
            pointLight.intensity = 0.4f;
            pointLight.color = new Color(0.99f, 0.73f, 0.05f);
            pointLight.range = 5;
            return lightPrefab;
        });
        light.Register();
    }
}