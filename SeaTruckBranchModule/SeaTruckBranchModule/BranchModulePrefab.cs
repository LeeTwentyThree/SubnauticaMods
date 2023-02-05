using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using System.Collections;
using UWE;

namespace SeaTruckBranchModule;

internal class BranchModulePrefab : Craftable
{
    public BranchModulePrefab() : base("SeaTruckBranchModule", "Seatruck Branch Module", "Attachable sea truck module with 4 connection points that allow for 2-dimensional expansion.")
    {
    }

    private GameObject prefab;

    private const string fabricatorModuleClassID = "dcf2d4f0-e121-4040-8f99-efb6c705df56";

    public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
    {
        if (prefab != null)
        {
            gameObject.Set(prefab);
            yield break;
        }

        var originalPrefabTask = PrefabDatabase.GetPrefabAsync(fabricatorModuleClassID);
        yield return originalPrefabTask;
        if (!originalPrefabTask.TryGetPrefab(out var fabricatorModulePrefab)) Plugin.Log.LogError("Failed to load Seatruck Fabricator Module prefab!");
        prefab = Object.Instantiate(fabricatorModulePrefab);
        prefab.SetActive(false);

        var modelParent = prefab.transform.Find("seatruck_module_fabricator_anim");

        var exteriorOld = modelParent.Find("Seatruck_module_Fabricator_exterior_geo");
        exteriorOld.gameObject.SetActive(false);
        var interiorOld = modelParent.Find("Seatruck_module_Fabricator_interior_geo");
        interiorOld.gameObject.SetActive(false);

        var mesh = Plugin.assetBundle.LoadAsset<GameObject>("SeaTruckBranchModule");
        var exterior = Object.Instantiate(mesh.transform.GetChild(0), modelParent, false);
        var exteriorRenderer = exterior.GetComponent<Renderer>();
        exteriorRenderer.materials = exteriorOld.GetComponent<Renderer>().materials;

        var interior = Object.Instantiate(mesh.transform.GetChild(1), modelParent, false);
        var interiorRenderer = interior.GetComponent<Renderer>();
        var oldInteriorMaterials = interiorOld.GetComponent<Renderer>().materials;
        interiorRenderer.materials = new Material[] { oldInteriorMaterials[0], oldInteriorMaterials[2], oldInteriorMaterials[1] };

        prefab.transform.Find("FabricatorSpawn").gameObject.SetActive(false);
        prefab.transform.Find("StorageContainer (1)").gameObject.SetActive(false);
        prefab.transform.Find("Label (4)").gameObject.SetActive(false);

        var backConnection = prefab.transform.Find("rearConnection");
        var rightConnection = Clone(backConnection);
        rightConnection.transform.localPosition = new Vector3(6.7f, 0, 1.65f);
        rightConnection.transform.localEulerAngles = Vector3.up * -90;
        var leftConnection = Clone(backConnection);
        leftConnection.transform.localPosition = new Vector3(6.7f, 0, 1.65f);
        leftConnection.transform.localEulerAngles = Vector3.up * 90;
        rightConnection.name = "rightConnection";
        leftConnection.name = "leftConnection";
        leftConnection.gameObject.GetComponent<ChildObjectIdentifier>().ClassId = "LeftConnection";
        rightConnection.gameObject.GetComponent<ChildObjectIdentifier>().ClassId = "RightConnection";
        var rightConnectionComponent = rightConnection.GetComponent<SeaTruckConnection>();
        var leftConnectionComponent = leftConnection.GetComponent<SeaTruckConnection>();

        var collision = prefab.transform.Find("collision");
        collision.Find("Cube (5)").gameObject.SetActive(false);
        collision.Find("Cube (7)").gameObject.SetActive(false);
        var entranceRef = collision.Find("entrance").gameObject;
        var leftEntrance = Clone(entranceRef, collision.transform).transform;
        var rightEntrance = Clone(entranceRef, collision.transform).transform;
        leftEntrance.transform.localPosition = new Vector3(-8, -1.39f, 6.85f);
        leftEntrance.transform.localEulerAngles = Vector3.up * 90;
        rightEntrance.transform.localPosition = new Vector3(-5.20f, -1.39f, 6.85f);
        rightEntrance.transform.localEulerAngles = Vector3.up * 90;

        var colorCustomizer = prefab.GetComponent<ColorCustomizer>();
        colorCustomizer.colorDatas[0].renderer = exteriorRenderer;
        colorCustomizer.colorDatas[1].renderer = interiorRenderer;
        colorCustomizer.colorDatas[2].renderer = interiorRenderer;
        colorCustomizer.colorDatas[2].materialIndex = 2;
        colorCustomizer.colorDatas=  colorCustomizer.colorDatas.AddRangeToArray(new ColorCustomizer.ColorData[]
        {
             new ColorCustomizer.ColorData(leftConnection.transform.GetChild(0).GetChild(0).GetComponent<Renderer>(), 0),
             new ColorCustomizer.ColorData(leftConnection.transform.GetChild(0).GetChild(1).GetComponent<Renderer>(), 0),
             new ColorCustomizer.ColorData(rightConnection.transform.GetChild(0).GetChild(0).GetComponent<Renderer>(), 0),
             new ColorCustomizer.ColorData(rightConnection.transform.GetChild(0).GetChild(1).GetComponent<Renderer>(), 0)
        });

        var skyAppliers = prefab.GetComponents<SkyApplier>();
        var interiorSkyApplier = skyAppliers[0];
        var glassSkyApplier = skyAppliers[1];
        var exteriorSkyApplier = skyAppliers[2];

        interiorSkyApplier.renderers[2] = interiorRenderer;
        interiorSkyApplier.renderers = interiorSkyApplier.renderers.AddRangeToArray(new Renderer[] {
            leftConnection.transform.GetChild(0).GetChild(1).GetComponent<Renderer>(),
            rightConnection.transform.GetChild(0).GetChild(1).GetComponent<Renderer>()
        });

        glassSkyApplier.renderers = glassSkyApplier.renderers.AddRangeToArray(new Renderer[] {
            leftConnection.transform.GetChild(0).GetChild(2).GetComponent<Renderer>(),
            leftConnection.transform.GetChild(0).GetChild(3).GetComponent<Renderer>(),
            rightConnection.transform.GetChild(0).GetChild(2).GetComponent<Renderer>(),
            rightConnection.transform.GetChild(0).GetChild(3).GetComponent<Renderer>()
        });

        exteriorSkyApplier.renderers[0] = exteriorRenderer;
        exteriorSkyApplier.renderers = exteriorSkyApplier.renderers.AddRangeToArray(new Renderer[] {
            leftConnection.transform.GetChild(0).GetChild(2).GetComponent<Renderer>(),
            leftConnection.transform.GetChild(0).GetChild(3).GetComponent<Renderer>(),
            rightConnection.transform.GetChild(0).GetChild(2).GetComponent<Renderer>(),
            rightConnection.transform.GetChild(0).GetChild(3).GetComponent<Renderer>()
        });

        var lightingController = prefab.GetComponent<LightingController>();
        var hashset = lightingController.emissiveController.renderers;
        hashset.Remove(interiorOld.GetComponent<Renderer>());
        hashset.Add(interiorRenderer);
        hashset.Remove(exteriorOld.GetComponent<Renderer>());
        hashset.Add(exteriorRenderer);
        hashset.AddRange(leftConnection.transform.GetChild(0).gameObject.GetComponentsInChildren<Renderer>());
        hashset.AddRange(rightConnection.transform.GetChild(0).gameObject.GetComponentsInChildren<Renderer>());

        var controller = prefab.AddComponent<SeaTruckBranchModule.BranchModuleController>();
        controller.left = leftConnectionComponent;
        controller.right = rightConnectionComponent;

        prefab.SetActive(true);
        gameObject.Set(prefab);
    }

    private GameObject Clone(GameObject original, Transform newParent = null)
    {
        if (newParent == null) newParent = prefab.transform;
        return Object.Instantiate(original, newParent, false);
    }

    private GameObject Clone(Transform original, Transform newParent = null)
    {
        if (newParent == null) newParent = prefab.transform;
        return Object.Instantiate(original.gameObject, newParent, false);
    }

    protected override RecipeData GetBlueprintRecipe()
    {
        return new RecipeData(
            new Ingredient(TechType.TitaniumIngot, 1), new Ingredient(TechType.Lead, 3)
            );
    }

    public override TechGroup GroupForPDA => TechGroup.Constructor;
    public override TechCategory CategoryForPDA => TechCategory.Constructor;
    public override string[] StepsToFabricatorTab => new[] { "Modules" };
    public override CraftTree.Type FabricatorType => CraftTree.Type.Constructor;
    public override float CraftingTime => 15f;


    protected override Sprite GetItemSprite()
    {
        return Plugin.assetBundle.LoadAsset<Sprite>("SeaTruckBranchModule_Icon");
    }
}
