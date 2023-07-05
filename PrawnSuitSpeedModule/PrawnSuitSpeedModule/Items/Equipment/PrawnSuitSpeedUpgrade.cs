using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Utility;
using System.IO;
using Ingredient = CraftData.Ingredient;

namespace PrawnSuitSpeedModule.Items.Equipment;
public static class PrawnSuitSpeedUpgrade
{
    public static PrefabInfo Info { get; } = PrefabInfo
        .WithTechType("PrawnSuitSpeedModule", "Prawn Suit Speed Module", "Powerful module that makes me go yes.")
        .WithIcon(ImageUtils.LoadSpriteFromFile(Path.Combine(Path.GetDirectoryName(Plugin.Assembly.Location), "Assets", "PrawnSuitSpeedModuleIcon.png")));

    public static void Register()
    {
        var customPrefab = new CustomPrefab(Info);

        var moduleModel = new CloneTemplate(Info, TechType.VehicleHullModule1);
        customPrefab.SetGameObject(moduleModel);
        customPrefab.SetRecipe(new RecipeData(new Ingredient(TechType.Copper, 2)));
        customPrefab.SetVehicleUpgradeModule(EquipmentType.ExosuitModule);
        customPrefab.Register();
    }
}