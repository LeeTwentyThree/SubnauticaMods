namespace AlterraFlux.Prefabs;

internal class CyclopsDockPrefab : IModPrefab
{
    public PrefabInfo Info { get; set; }

    public TechType Register()
    {
        Info = PrefabInfo.WithTechType("CyclopsDockingBay", "Cyclops Docking Bay", "Big dock.")
    .WithIcon(SpriteManager.Get(TechType.BaseMoonpool));
        CustomPrefab prefab = new CustomPrefab(Info);
        prefab.SetPrefab(GetGameObject);
        prefab.SetRecipe(new RecipeData(new[] { new Ingredient(TechType.PlasteelIngot, 4) }));
        CraftDataHandler.AddToGroup(TechGroup.ExteriorModules, TechCategory.ExteriorModule, Info.TechType);
        prefab.Register();
        return Info.TechType;
    }

    public IEnumerator GetGameObject(IOut<GameObject> gameObject)
    {
        var prefab = Object.Instantiate(Main.assetBundle.LoadAsset<GameObject>("CyclopsDockingBayPrefab"));
        prefab.SetActive(false);
        PrefabUtils.AddBasicComponents(prefab, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Global);
        prefab.AddComponent<Rigidbody>().isKinematic = true;
        PrefabUtils.AddConstructable(prefab, Info.TechType,
            ConstructableFlags.Outside | ConstructableFlags.Rotatable | ConstructableFlags.Ground | ConstructableFlags.AllowedOnConstructable
        );
        var constructable = prefab.GetComponent<Constructable>();
        constructable.model = prefab.transform.GetChild(0).gameObject;
        constructable.forceUpright = true;
        constructable.placeMinDistance = 4f;
        constructable.placeDefaultDistance = 20f;
        constructable.placeMaxDistance = 40f;

        var ghostMaterialTask = new TaskResult<Material>();
        yield return CommonPrefabs.GetGhostMaterial(ghostMaterialTask);
        constructable.ghostMaterial = ghostMaterialTask.Get();

        prefab.AddComponent<ConstructableBounds>().bounds = new OrientedBounds(Vector3.up * 21, Quaternion.identity, new Vector3(18, 16, 54) / 2);

        MaterialUtils.ApplySNShaders(prefab);

        var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        var planeMesh = plane.GetComponent<MeshFilter>().mesh;
        Object.Destroy(plane);

        var waterSurfaceTask = new TaskResult<GameObject>();
        yield return CommonPrefabs.GetMoonpoolWaterSurface(waterSurfaceTask);
        var surface = Object.Instantiate(waterSurfaceTask.Get());
        surface.transform.parent = prefab.transform;
        surface.transform.localPosition = Vector3.up * 14.9f;
        surface.transform.localRotation = Quaternion.identity;
        surface.transform.localScale = new Vector3(2.1f, 1f, 5.52f);
        surface.GetComponent<MeshFilter>().mesh = planeMesh;
        var waterRenderer = surface.GetComponent<MeshRenderer>();
        var matWater = waterRenderer.material;
        matWater.color = new Color(.48f, .72f, .86f, 0);
        matWater.SetVector("_Scale", new Vector4(0, .4f, 0, 0));
        matWater.SetVector("_TexScale", new Vector4(.02f, 1, .15f, .2f));
        matWater.SetFloat("_MainFoam", .15f);
        matWater.SetFloat("_TopFoamHeight", 2.73f);
        waterRenderer.material = matWater;

        var dock = prefab.AddComponent<CyclopsDock>();
        dock.door1 = prefab.SearchChild("Door1").AddComponent<CyclopsDockDoor>();
        dock.door2 = prefab.SearchChild("Door2").AddComponent<CyclopsDockDoor>();
        dock.waterPlane = surface.transform;

        prefab.SetActive(true);
        gameObject.Set(prefab);
    }
}
