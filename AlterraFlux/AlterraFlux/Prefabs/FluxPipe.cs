namespace AlterraFlux.Prefabs;

internal class FluxPipe : IModPrefab
{
    public PrefabInfo Info { get; private set; }

    public IEnumerator GetGameObject(IOut<GameObject> gameObject)
    {
        var prefab = new GameObject("FluxPipe");
        prefab.SetActive(false);
        PrefabUtils.AddBasicComponents(prefab, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Global);
        var model = new GameObject("Model");
        model.transform.SetParent(prefab.transform);
        PrefabUtils.AddConstructable(prefab, Info.TechType,
            ConstructableFlags.Outside | ConstructableFlags.Rotatable | ConstructableFlags.AllowedOnConstructable | ConstructableFlags.Ground);
        var constructable = prefab.GetComponent<Constructable>();
        constructable.model = model;
        constructable.placeMinDistance = 1;
        constructable.placeDefaultDistance = 10;
        constructable.placeMaxDistance = 20;

        model.gameObject.AddComponent<PipePlacementStandIn>();

        yield return null;
        prefab.SetActive(true);
        gameObject.Set(prefab);
    }

    public TechType Register()
    {
        Info = PrefabInfo.WithTechType("FluxPipe", "Flux Pipe", "Allows for the transporation of items.")
            .WithIcon(SpriteManager.Get(TechType.Pipe));
        CustomPrefab prefab = new CustomPrefab(Info);
        prefab.SetRecipe(new RecipeData(new Ingredient(TechType.Quartz, 1)));
        CraftDataHandler.AddToGroup(TechGroup.ExteriorModules, TechCategory.ExteriorModule, Info.TechType);
        prefab.SetPrefab(GetGameObject);
        prefab.Register();
        return Info.TechType;
    }
}