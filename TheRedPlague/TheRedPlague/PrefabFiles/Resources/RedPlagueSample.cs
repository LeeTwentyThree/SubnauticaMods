using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;

namespace TheRedPlague.PrefabFiles.Resources;

public static class RedPlagueSample
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("RedPlagueSample")
        .WithIcon(SpriteManager.Get(TechType.LabEquipment3))
        .WithSizeInInventory(new Vector2int(2, 2));

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        var template = new CloneTemplate(Info, TechType.LabEquipment3);
        prefab.SetGameObject(template);
        prefab.Register();
    }
}