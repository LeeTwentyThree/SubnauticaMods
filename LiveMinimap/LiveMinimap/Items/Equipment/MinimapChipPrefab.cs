using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Ingredient = CraftData.Ingredient;

namespace LiveMinimap.Items.Equipment;
public static class MinimapChipPrefab
{
    public static PrefabInfo Info { get; } = PrefabInfo
        .WithTechType("LiveMinimapChip", "Minimap HUD chip", "Minimap chip that makes me go yes.")
        .WithIcon(SpriteManager.Get(TechType.MapRoomHUDChip));

    public static void Register()
    {
        var customPrefab = new CustomPrefab(Info);

        var model = new CloneTemplate(Info, TechType.MapRoomHUDChip);
        customPrefab.SetGameObject(model);
        customPrefab.SetRecipe(new RecipeData(new Ingredient(TechType.Diamond, 2), new Ingredient(TechType.Magnetite, 1)))
            .WithFabricatorType(CraftTree.Type.Fabricator)
            .WithStepsToFabricatorTab("Personal", "Equipment");
        customPrefab.SetEquipment(EquipmentType.Chip);
        customPrefab.SetPdaGroupCategory(TechGroup.Personal, TechCategory.Equipment);
        customPrefab.Register();
    }
}