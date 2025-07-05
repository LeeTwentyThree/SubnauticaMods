using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Utility;
using System.Collections;
using System.Collections.Generic;
using SeaVoyager.Mono;
using UnityEngine;
using UWE;
using static CraftData;

namespace SeaVoyager.Prefabs.Vehicles;

public class SeaVoyagerPrefab
{
    public PrefabInfo Info { get; } = PrefabInfo.WithTechType("SeaVoyager", "Sea Voyager", "A large self-sustaining ship that specializes in quick transportation.");

    public SeaVoyagerPrefab Register()
    {
        var prefab = new CustomPrefab(Info);

        Info.WithIcon(Plugin.assetBundle.LoadAsset<Sprite>("SeaVoyagerCraftIcon"));

        var craftingGadget = prefab.SetRecipe(new Nautilus.Crafting.RecipeData(
            new Ingredient(TechType.TitaniumIngot, 2),
            new Ingredient(TechType.Lubricant, 1),
            new Ingredient(TechType.Floater, 2),
            new Ingredient(TechType.WiringKit, 1),
            new Ingredient(TechType.Glass, 1)
        ));

        craftingGadget
            .WithStepsToFabricatorTab("Vehicles")
            .WithCraftingTime(10f)
            .WithFabricatorType(CraftTree.Type.Constructor);

        prefab.SetPdaGroupCategory(TechGroup.Constructor, TechCategory.Constructor)
            .WithAnalysisTech(null)
            .WithEncyclopediaEntry("Tech/Vehicles", null)
            .RequiredForUnlock = Fragments.SeaVoyagerFragment.SeaVoyagerFragmentTechType;

        prefab.SetGameObject(GetGameObjectAsync);

        prefab.Register();

        return this;
    }

    public IEnumerator GetGameObjectAsync(IOut<GameObject> returnedPrefab)
    {
        // Load the model
        var prefab = Object.Instantiate(Plugin.assetBundle.LoadAsset<GameObject>("SeaVoyagerPrefab"));
        prefab.SetActive(false);

        // Add essential components
        PrefabUtils.AddBasicComponents(prefab, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Global);

        // Load the rocket platform for reference. I only use it for the constructor animation and sounds.
        var rocketPlatformRequest = GetPrefabForTechTypeAsync(TechType.RocketBase);
        yield return rocketPlatformRequest;
        var rocketPlatformReference = rocketPlatformRequest.GetResult();

        // Get glass material
        Material glassMaterial = null;

        var aquariumTask = GetPrefabForTechTypeAsync(TechType.Aquarium);
        yield return aquariumTask;
        var aquariumModel = aquariumTask.GetResult();

        Renderer[] renderers = aquariumModel.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            foreach (Material material in renderer.materials)
            {
                if (material.name.ToLower().Contains("glass"))
                {
                    glassMaterial = material;
                    break;
                }
            }
        }
        
        // Load the base prefab
        var baseTask = PrefabDatabase.GetPrefabAsync("e9b75112-f920-45a9-97cc-838ee9b389bb");
        yield return baseTask;
        if (!baseTask.TryGetPrefab(out var basePrefab))
        {
            Plugin.Logger.LogError("Failed to load base prefab!");
            yield break;
        }

        // Apply materials. It got so long and ugly that I made it its own method.
        MaterialUtils.ApplySNShaders(prefab);
        prefab.SearchChild("Window").GetComponent<MeshRenderer>().material = glassMaterial;

        // Get the Transform of the models
        Transform interiorModels = Helpers.FindChild(prefab, "Interior").transform;
        Transform exteriorModels = Helpers.FindChild(prefab, "Exterior").transform; // I never actually use this reference.
        // Adds a Rigidbody. So it can move.
        var rigidbody = prefab.AddComponent<Rigidbody>();
        rigidbody.mass = 20000f; // Has to be really heavy. Measured in kilograms.
        rigidbody.useGravity = false; // Can't believe I didn't catch this one in the 1.0 version...
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        // Basically an extension to Unity rigidbodys. Necessary for buoyancy.
        var worldForces = prefab.AddComponent<WorldForces>();
        worldForces.useRigidbody = rigidbody;
        worldForces.underwaterGravity = -5f; // Despite it being negative, which would apply downward force, this actually makes it go UP on the y axis.
        worldForces.aboveWaterGravity = 5f; // Counteract the strong upward force
        worldForces.waterDepth = -5f;
        // Determines the places the little build bots point their laser beams at.
        var buildBots = prefab.AddComponent<BuildBotBeamPoints>();
        Transform beamPointsParent = Helpers.FindChild(prefab, "BuildBotPoints").transform;
        // These are arbitrarily placed.
        buildBots.beamPoints = new Transform[beamPointsParent.childCount];
        for (int i = 0; i < beamPointsParent.childCount; i++)
        {
            buildBots.beamPoints[i] = beamPointsParent.GetChild(i);
        }
        // The path the build bots take to get to the ship to construct it.
        Transform pathsParent = Helpers.FindChild(prefab, "BuildBotPaths").transform;
        // 4 paths, one for each build bot to take.
        CreateBuildBotPath(prefab, pathsParent.GetChild(0));
        CreateBuildBotPath(prefab, pathsParent.GetChild(1));
        CreateBuildBotPath(prefab, pathsParent.GetChild(2));
        CreateBuildBotPath(prefab, pathsParent.GetChild(3));
        // The effects for the constructor.
        var vfxConstructing = prefab.AddComponent<VFXConstructing>();
        var rocketPlatformVfx = rocketPlatformReference.GetComponentInChildren<VFXConstructing>();
        vfxConstructing.ghostMaterial = rocketPlatformVfx.ghostMaterial;
        vfxConstructing.surfaceSplashSound = rocketPlatformVfx.surfaceSplashSound;
        vfxConstructing.surfaceSplashFX = rocketPlatformVfx.surfaceSplashFX;
        vfxConstructing.Regenerate();
        // Don't want it tipping over...
        var stabilizier = prefab.AddComponent<Stabilizer>();
        stabilizier.uprightAccelerationStiffness = 40f;
        // Some components might need this. I don't WANT it to take damage though, so I will just give it a LOT of health.
        var liveMixin = prefab.AddComponent<LiveMixin>();
        var lmData = ScriptableObject.CreateInstance<LiveMixinData>();
        lmData.canResurrect = true;
        lmData.broadcastKillOnDeath = false;
        lmData.destroyOnDeath = false;
        lmData.invincibleInCreative = true;
        lmData.weldable = true;
        lmData.minDamageForSound = 20f;
        lmData.maxHealth = 250000000000000000f;
        liveMixin.data = lmData;
        // I don't know if this does anything at all as ships float above the surface, but I'm keeping it.
        var oxygenManager = prefab.AddComponent<OxygenManager>();
        // I don't understand why I'm doing this, but I will anyway. The power cell is nowhere to be seen. To avoid learning how the EnergyMixin code works, I just added an external solar panel that stores all the power anyway.
        var energyMixin = prefab.AddComponent<EnergyMixin>();
        energyMixin.compatibleBatteries = new List<TechType>() { TechType.PowerCell, TechType.PrecursorIonPowerCell };
        energyMixin.defaultBattery = TechType.PowerCell;
        energyMixin.storageRoot = Helpers.FindChild(prefab, "StorageRoot").AddComponent<ChildObjectIdentifier>();
        // Allows power to connect to here.
        var powerRelay = prefab.AddComponent<PowerRelay>();
        // Necessary for SubRoot class Update behaviour so it doesn't return an error every frame.
        var lod = prefab.AddComponent<BehaviourLOD>();
        // Add VFXSurfaces to adjust footstep sounds. This is technically not necessary for the interior colliders, however.
        foreach (Collider col in prefab.GetComponentsInChildren<Collider>())
        {
            var vfxSurface = col.gameObject.AddComponent<VFXSurface>();
            vfxSurface.surfaceType = VFXSurfaceTypes.metal;
        }
        // The SubRoot component needs a lighting controller. Works nice too. A pain to setup in script.
        var lights = Helpers.FindChild(prefab, "LightsParent").AddComponent<LightingController>();
        lights.lights = new MultiStatesLight[0];
        foreach (Transform child in lights.transform)
        {
            var newLight = new MultiStatesLight();
            newLight.light = child.GetComponent<Light>();
            newLight.intensities = new[] { 1f, 0.5f, 0f }; //Full power: intensity 1. Emergency : intensity 0.5. No power: intensity 0.
            lights.RegisterLight(newLight);
        }
        // Sky appliers to make it look nicer. Not sure if it even makes a difference, but I'm sticking with it.
        var skyApplierInterior = interiorModels.gameObject.AddComponent<SkyApplier>();
        skyApplierInterior.renderers = interiorModels.GetComponentsInChildren<Renderer>();
        skyApplierInterior.anchorSky = Skies.BaseInterior;
        skyApplierInterior.SetSky(Skies.BaseInterior);
        skyApplierInterior.lightControl = lights;
        
        // Add entrance door
        var door = prefab.transform.Find("Model/Exterior/MainDoor/DoorCollider").gameObject;
        Plugin.Logger.LogDebug($"{door}");
        var hatch = door.AddComponent<UseableDiveHatch>();
        hatch.insideSpawn = prefab.transform.Find("Model/Exterior/MainDoor/EnterPosition").gameObject;
        hatch.outsideExit = prefab.transform.Find("Model/Exterior/MainDoor/ExitPosition").gameObject;
        hatch.enterCustomText = "SeaVoyager_Enter";
        hatch.exitCustomText = "SeaVoyager_Exit";
        hatch.ignoreObject = prefab;
        
        // Load a seamoth for reference
        var seamothRequest = GetPrefabForTechTypeAsync(TechType.Seamoth);
        yield return seamothRequest;
        GameObject seamothRef = seamothRequest.GetResult();

        // Get the seamoth's water clip proxy component. This is what displaces the water.
        var seamothProxy = seamothRef.GetComponentInChildren<WaterClipProxy>();
        // Find the parent of all the ship's clip proxys.
        Transform proxyParent = Helpers.FindChild(prefab, "ClipProxys").transform;

        // Loop through them all
        foreach (Transform child in proxyParent)
        {
            var waterClip = child.gameObject.AddComponent<WaterClipProxy>();
            waterClip.shape = WaterClipProxy.Shape.Box;
            // Apply the seamoth's clip material. No idea what shader it uses or what settings it actually has, so this is an easier option. Reuse the game's assets.
            waterClip.clipMaterial = seamothProxy.clipMaterial;
            // You need to do this. By default the layer is 0. This makes it displace everything in the default rendering layer. We only want to displace water.
            waterClip.gameObject.layer = seamothProxy.gameObject.layer;
        }

        // Arbitrary number. The ship doesn't have batteries anyway.
        energyMixin.maxEnergy = 1200f;


        // Add the SeaVoyager component. Inherits from SubRoot, the same component that both the cyclops submarine and bases use.
        var shipBehaviour = prefab.AddComponent<Mono.SeaVoyager>();
        shipBehaviour.worldForces = worldForces;

        // It needs to produce power somehow
        var basePowerRelay = basePrefab.GetComponent<PowerRelay>();
        powerRelay.powerSystemPreviewPrefab = basePowerRelay.powerSystemPreviewPrefab;
        
        var powerCellsParent = new GameObject("PowerCellsParent").transform;
        powerCellsParent.SetParent(prefab.transform, false);
        powerCellsParent.localPosition = new Vector3(0, -15, 0);
        powerCellsParent.localEulerAngles = Vector3.zero;
        powerCellsParent.gameObject.AddComponent<ChildObjectIdentifier>().ClassId = "PocketDimensionPower";

        var placeholdersGroup = prefab.AddComponent<PrefabPlaceholdersGroup>();
        var powerCellLocations = new[]{ prefab.gameObject.transform.position + new Vector3(0,15,0),  prefab.gameObject.transform.position + new Vector3(1, 15, 0) };

        var placeholders = new PrefabPlaceholder[powerCellLocations.Length];
        
        for (int i = 0; i < powerCellLocations.Length; i++)
        {
            var placeholder = new GameObject("PowerCellPlaceholder");
            var placeholderComponent = placeholder.AddComponent<PrefabPlaceholder>();
            placeholderComponent.prefabClassId = "0cb22d0e-ba5e-4e4b-b7a7-a67931fb5e0c";
            placeholder.transform.SetParent(powerCellsParent, false);
            placeholder.transform.localPosition = powerCellLocations[i];
            placeholder.transform.localRotation = Quaternion.identity;
            placeholders[i] = placeholderComponent;
        }
        placeholdersGroup.prefabPlaceholders = placeholders;

        // A ping so you can see it from far away
        var ping = prefab.AddComponent<PingInstance>();
        ping.pingType = Plugin.SeaVoyagerPingType;
        ping.origin = Helpers.FindChild(prefab, "PingOrigin").transform;

        // Add a respawn point
        var respawnPoint = Helpers.FindChild(prefab, "RespawnPoint").AddComponent<RespawnPoint>();

        // Motor sound
        var motorSound = Helpers.FindChild(prefab, "EngineLoop").AddComponent<ShipMotorSound>();
        motorSound.emitter = motorSound.gameObject.AddComponent<FMOD_CustomLoopingEmitter>();
        motorSound.emitter.followParent = true;
        motorSound.seaVoyager = shipBehaviour;

        // Voice
        var shipVoice = prefab.AddComponent<ShipVoice>();
        var voiceSource = prefab.AddComponent<AudioSource>();
        // voiceSource.volume = Plugin.config.NormalizedAudioVolume;
        shipVoice.source = voiceSource;
        shipBehaviour.voice = shipVoice;

        // Shallow water scanner
        var shallowWaterScanner = Helpers.FindChild(prefab, "ShallowWaterScanner").AddComponent<ShipShallowWaterScanner>();
        shallowWaterScanner.seaVoyager = shipBehaviour;

        // Power depletion notification
        var shipPowerWarning = prefab.AddComponent<ShipPowerWarning>();
        shipPowerWarning.ship = shipBehaviour;

        // Auto-stop for when you fall off
        var autoStop = prefab.AddComponent<ShipAutoStop>();
        autoStop.ship = shipBehaviour;

        // Cool window in the bedroom
        var bedRoomWindow = Helpers.FindChild(prefab, "BedRoomWindow").AddComponent<BedRoomWindow>();
        bedRoomWindow.SetupPrefab(shipBehaviour);

        // Make sure you don't walk on the seafloor
        var walkableAreaBounds = prefab.AddComponent<ShipWalkableAreaBounds>();
        walkableAreaBounds.ship = shipBehaviour;
        returnedPrefab.Set(prefab);
    }

    BuildBotPath CreateBuildBotPath(GameObject gameobjectWithComponent, Transform parent)
    {
        var comp = gameobjectWithComponent.AddComponent<BuildBotPath>();
        comp.points = new Transform[parent.childCount];
        for (int i = 0; i < parent.childCount; i++)
        {
            comp.points[i] = parent.GetChild(i);
        }
        return comp;
    }
}
