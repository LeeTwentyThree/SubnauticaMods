using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Utility;
using PrawnSuitSpeedModule.Mono;
using System.IO;
using Ingredient = CraftData.Ingredient;

namespace PrawnSuitSpeedModule.Items.Equipment;
public static class PrawnSuitSpeedUpgrade
{
    public static PrefabInfo Info { get; } = PrefabInfo
        .WithTechType("PrawnSuitSpeedModule", "Prawn Suit Speed Module", "Allows the Prawn Suit walking mechanism to enter overdrive. Multiple modules may be installed simultaneously.")
        .WithIcon(ImageUtils.LoadSpriteFromFile(Path.Combine(Path.GetDirectoryName(Plugin.Assembly.Location), "Assets", "PrawnSuitSpeedModuleIcon.png")));

    public static void Register()
    {
        var customPrefab = new CustomPrefab(Info);

        var moduleModel = new CloneTemplate(Info, TechType.VehicleHullModule1);
        customPrefab.SetGameObject(moduleModel);
        customPrefab.SetRecipe(new RecipeData(new Ingredient(TechType.Nickel, 2), new Ingredient(TechType.Polyaniline, 1)))
            .WithFabricatorType(CraftTree.Type.SeamothUpgrades)
            .WithStepsToFabricatorTab("ExosuitModules");
        customPrefab.SetVehicleUpgradeModule(EquipmentType.ExosuitModule)
            .WithOnModuleAdded(OnUpdateModule) 
            .WithOnModuleRemoved(OnUpdateModule); 
        customPrefab.Register();
    }

    private static void OnUpdateModule(Vehicle vehicle, int num)
    {
        vehicle.gameObject.EnsureComponent<PrawnSuitSpeedHandler>().UpdateSpeed();
    }
}