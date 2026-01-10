using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;

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
            }
        });
        prefab.Register();
    }
}