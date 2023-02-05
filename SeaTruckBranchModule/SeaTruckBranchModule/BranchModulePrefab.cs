using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using System.Collections;
using UWE;

namespace SeaTruckExpansionModule;

internal class BranchModulePrefab : PdaItem
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

        var skyAppliers = prefab.GetComponents<SkyApplier>();
        var interiorSkyApplier = skyAppliers[0];
        var glassSkyApplier = skyAppliers[1];
        var exteriorSkyApplier = skyAppliers[2];

        interiorSkyApplier.renderers[2] = interiorRenderer;

        exteriorSkyApplier.renderers[0] = exteriorRenderer;

        var lightingController = prefab.GetComponent<LightingController>();
        var hashset = lightingController.emissiveController.renderers;
        hashset.Remove(interiorOld.GetComponent<Renderer>());
        hashset.Add(interiorRenderer);
        hashset.Remove(exteriorOld.GetComponent<Renderer>());
        hashset.Add(exteriorRenderer);

        var colorCustomizer = prefab.GetComponent<ColorCustomizer>();
        colorCustomizer.colorDatas[0].renderer = exteriorRenderer;
        colorCustomizer.colorDatas[1].renderer = interiorRenderer;
        colorCustomizer.colorDatas[2].renderer = interiorRenderer;
        colorCustomizer.colorDatas[2].materialIndex = 2;

        var frontConnection = prefab.transform.Find("frontConnection");
        var rightConnection = Clone(frontConnection);
        rightConnection.transform.localPosition = new Vector3(-1.9f, 0, 1.65f);
        rightConnection.transform.localEulerAngles = Vector3.up * 90;
        var leftConnection = Clone(frontConnection);
        leftConnection.transform.localPosition = new Vector3(1.9f, 0, 1.65f);
        leftConnection.transform.localEulerAngles = Vector3.up * -90;

        var collision = prefab.transform.Find("collision");
        collision.Find("Cube (5)").gameObject.SetActive(false);
        collision.Find("Cube (7)").gameObject.SetActive(false);
        var entranceRef = collision.Find("entrance").gameObject;
        var leftEntrance = Clone(entranceRef, collision.transform).transform;
        var rightEntrance = Clone(entranceRef, collision.transform).transform;

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
}
