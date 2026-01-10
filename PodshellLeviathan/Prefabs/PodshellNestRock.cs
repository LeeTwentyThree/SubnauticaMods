using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using UnityEngine;

namespace PodshellLeviathan.Prefabs;

public static class PodshellNestRock
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("PodshellNestRock");

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(new CloneTemplate(Info, "fa986d5a-0cf8-4c63-af9f-8c36acd5bea4")
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
    }
}