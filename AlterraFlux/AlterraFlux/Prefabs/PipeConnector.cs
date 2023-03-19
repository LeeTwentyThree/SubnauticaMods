namespace AlterraFlux.Prefabs;

internal class PipeConnector : IModPrefab
{
    public PrefabInfo Info { get; private set; }

    public readonly string techType;
    public readonly string name;
    public readonly string description;
    public readonly string prefabAssetName;

    public PipeConnector(string techType, string name, string description, string prefabAssetName)
    {
        this.techType = techType;
        this.name = name;
        this.description = description;
        this.prefabAssetName = prefabAssetName;
    }

    public IEnumerator GetGameObject(IOut<GameObject> gameObject)
    {
        var prefab = Object.Instantiate(Main.assetBundle.LoadAsset<GameObject>(prefabAssetName));
        PrefabUtils.AddBasicComponents(prefab, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Global);
        prefab.AddComponent<Rigidbody>().isKinematic = true;
        PrefabUtils.AddConstructable(prefab, Info.TechType,
            ConstructableFlags.Outside | ConstructableFlags.Rotatable | ConstructableFlags.Ground | ConstructableFlags.AllowedOnConstructable
        );
        var pivot = prefab.transform.GetChild(0);
        var constructable = prefab.GetComponent<Constructable>();
        constructable.model = pivot.gameObject;
        constructable.placeMinDistance = 4;
        constructable.placeDefaultDistance = 4;
        constructable.placeMaxDistance = 10;

        pivot.localScale = Vector3.one * GlobalData.pipeConnectorScale;

        int x = 0;
        foreach (Transform child in pivot)
        {
            if (child.name.Contains("ConnectionPoint"))
            {
                AddPipeConnectionPoint(child, x);
                x++;
            }
        }

        var ghostMaterialTask = new TaskResult<Material>();
        yield return CommonPrefabs.GetGhostMaterial(ghostMaterialTask);
        constructable.ghostMaterial = ghostMaterialTask.Get();

        MaterialUtils.ApplySNShaders(prefab, 4, 1, 1, new SMLHelper.Utility.MaterialModifiers.ColorModifier(new Color(3, 3, 3)));

        yield return null;
        gameObject.Set(prefab);
    }

    private void AddPipeConnectionPoint(Transform point, int id)
    {
        var c = point.gameObject.AddComponent<PipeConnectionPoint>();
        c.relativeId = id;
    }

    public TechType Register()
    {
        Info = PrefabInfo.WithTechType(techType, name, description)
.WithIcon(SpriteManager.Get(TechType.PipeSurfaceFloater));
        CustomPrefab prefab = new CustomPrefab(Info);
        prefab.SetPrefab(GetGameObject);
        prefab.SetRecipe(new RecipeData(new[] { new Ingredient(TechType.PlasteelIngot, 4) }));
        CraftDataHandler.AddToGroup(TechGroup.ExteriorModules, TechCategory.ExteriorModule, Info.TechType);
        prefab.Register();
        return Info.TechType;
    }
}