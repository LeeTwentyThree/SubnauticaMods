using System.Collections;
using HarmonyLib;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using UnityEngine;
using UWE;

namespace SeaTruckBranchModule;

internal static class BranchModulePrefab
{
    private const string FabricatorModuleClassID = "dcf2d4f0-e121-4040-8f99-efb6c705df56";

    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("SeaTruckBranchModule")
        .WithIcon(Plugin.Bundle.LoadAsset<Sprite>("SeaTruckBranchModule_Icon"));

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(CreatePrefab);
        prefab.SetRecipe(new RecipeData(
                new Ingredient(TechType.PlasteelIngot, 1), new Ingredient(TechType.Lead, 1), new Ingredient(TechType.Silicone, 1)))
            .WithCraftingTime(15)
            .WithFabricatorType(CraftTree.Type.Constructor)
            .WithStepsToFabricatorTab("Modules");
        prefab.SetPdaGroupCategory(TechGroup.Constructor, TechCategory.Constructor);
        prefab.SetUnlock(TechType.SeaTruck);
        prefab.Register();
    }

    private static IEnumerator CreatePrefab(IOut<GameObject> gameObject)
    {
        var originalPrefabTask = PrefabDatabase.GetPrefabAsync(FabricatorModuleClassID);
        yield return originalPrefabTask;
        if (!originalPrefabTask.TryGetPrefab(out var fabricatorModulePrefab))
        {
            Plugin.Logger.LogError("Failed to load Seatruck Fabricator Module prefab!");
            yield break;
        }

        var prefab = Object.Instantiate(fabricatorModulePrefab);
        prefab.SetActive(false);

        var modelParent = prefab.transform.Find("seatruck_module_fabricator_anim");

        var exteriorOld = modelParent.Find("Seatruck_module_Fabricator_exterior_geo");
        exteriorOld.gameObject.SetActive(false);
        var interiorOld = modelParent.Find("Seatruck_module_Fabricator_interior_geo");
        interiorOld.gameObject.SetActive(false);

        var mesh = Plugin.Bundle.LoadAsset<GameObject>("SeaTruckBranchModule");
        var exterior = Object.Instantiate(mesh.transform.GetChild(0), modelParent, false);
        var exteriorRenderer = exterior.GetComponent<Renderer>();
        exteriorRenderer.materials = exteriorOld.GetComponent<Renderer>().materials;

        var interior = Object.Instantiate(mesh.transform.GetChild(1), modelParent, false);
        var interiorRenderer = interior.GetComponent<Renderer>();
        var oldInteriorMaterials = interiorOld.GetComponent<Renderer>().materials;
        interiorRenderer.materials = new Material[]
            { oldInteriorMaterials[0], oldInteriorMaterials[2], oldInteriorMaterials[1] };

        prefab.transform.Find("FabricatorSpawn").gameObject.SetActive(false);
        prefab.transform.Find("StorageContainer (1)").gameObject.SetActive(false);
        prefab.transform.Find("Label (4)").gameObject.SetActive(false);

        var backConnection = prefab.transform.Find("rearConnection");
        var rightConnection = Clone(backConnection, prefab.transform);
        rightConnection.transform.localPosition = new Vector3(6.7f, 0, 1.65f);
        rightConnection.transform.localEulerAngles = Vector3.up * -90;
        var leftConnection = Clone(backConnection, prefab.transform);
        leftConnection.transform.localPosition = new Vector3(-6.7f, 0, 1.65f);
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
        colorCustomizer.colorDatas = colorCustomizer.colorDatas.AddRangeToArray(new ColorCustomizer.ColorData[]
        {
            new(leftConnection.transform.GetChild(0).GetChild(0).GetComponent<Renderer>(), 0),
            new(leftConnection.transform.GetChild(0).GetChild(1).GetComponent<Renderer>(), 0),
            new(rightConnection.transform.GetChild(0).GetChild(0).GetComponent<Renderer>(), 0),
            new(rightConnection.transform.GetChild(0).GetChild(1).GetComponent<Renderer>(), 0)
        });

        var skyAppliers = prefab.GetComponents<SkyApplier>();
        var interiorSkyApplier = skyAppliers[0];
        var glassSkyApplier = skyAppliers[1];
        var exteriorSkyApplier = skyAppliers[2];

        interiorSkyApplier.renderers[2] = interiorRenderer;
        interiorSkyApplier.renderers = interiorSkyApplier.renderers.AddRangeToArray(new[]
        {
            leftConnection.transform.GetChild(0).GetChild(1).GetComponent<Renderer>(),
            rightConnection.transform.GetChild(0).GetChild(1).GetComponent<Renderer>()
        });

        glassSkyApplier.renderers = glassSkyApplier.renderers.AddRangeToArray(new[]
        {
            leftConnection.transform.GetChild(0).GetChild(2).GetComponent<Renderer>(),
            leftConnection.transform.GetChild(0).GetChild(3).GetComponent<Renderer>(),
            rightConnection.transform.GetChild(0).GetChild(2).GetComponent<Renderer>(),
            rightConnection.transform.GetChild(0).GetChild(3).GetComponent<Renderer>()
        });

        exteriorSkyApplier.renderers[0] = exteriorRenderer;
        exteriorSkyApplier.renderers = exteriorSkyApplier.renderers.AddRangeToArray(new[]
        {
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

        var controller = prefab.AddComponent<BranchModuleController>();
        controller.left = leftConnectionComponent;
        controller.right = rightConnectionComponent;
        controller.rear = backConnection.gameObject.GetComponent<SeaTruckConnection>();

        prefab.SetActive(true);
        gameObject.Set(prefab);
    }

    private static GameObject Clone(GameObject original, Transform newParent)
    {
        return Object.Instantiate(original, newParent, false);
    }

    private static GameObject Clone(Transform original, Transform newParent)
    {
        return Object.Instantiate(original.gameObject, newParent, false);
    }
}