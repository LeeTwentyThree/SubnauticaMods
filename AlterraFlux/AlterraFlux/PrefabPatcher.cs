using AlterraFlux.API;
using AlterraFlux.Prefabs;

namespace AlterraFlux;

internal static class PrefabPatcher
{
    public static void Register()
    {
        PrefabInfo quarryInfo = PrefabInfo.WithTechType("QuarryExtractor", "Quarry Extractor", "Digs a lot.")
            .WithIcon(SpriteManager.Get(TechType.ThermalPlant));
        CustomPrefab quarry = new CustomPrefab(quarryInfo);
        quarry.SetPrefab(new CloneTemplate(quarryInfo, TechType.ThermalPlant));
        quarry.SetRecipe(new RecipeData(new[] { new Ingredient(TechType.Titanium, 1) }));
        CraftDataHandler.AddToGroup(TechGroup.ExteriorModules, TechCategory.ExteriorModule, quarryInfo.TechType);
        quarry.Register();
        TechTypes.QuarryExtractor = quarryInfo.TechType;

        TechTypes.CyclopsDockingBay = new CyclopsDockPrefab().Register();

        RegisterPipeConnector("Pipe_3to1");
        RegisterPipeConnector("Pipe_Arc90");
        RegisterPipeConnector("Pipe_Arc180");
        RegisterPipeConnector("Pipe_Small");
        RegisterPipeConnector("Pipe_TJoint");
        RegisterPipeConnector("Pipe_Valve");
        RegisterPipeConnector("Pipe_XJoint");
        RegisterPipeConnector("Pipe_YJoint");

        TechTypes.FluxPipe = new FluxPipe().Register();
    }

    private static void RegisterPipeConnector(string assetName)
    {
        new PipeConnector(assetName, assetName, assetName + " that makes me go yes!", assetName).Register();
    }
}
