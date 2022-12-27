using System;
using System.IO;
using System.Reflection;
using QModManager.API.ModLoading;
using SMLHelper.V2.Utility;
using UnityEngine;
using ShipMod.Ship;
using ShipMod.Prefabs;
using HarmonyLib;
using SMLHelper.V2.Handlers;
using System.Collections;
using System.Collections.Generic;
using UWE;

namespace ShipMod
{
    [QModCore]
    public static class QPatch
    {
        public static AssetBundle bundle;
        public static ShipPrefab seaVoyagerPrefab;
        public static PingType shipPingType;

        public static Atlas.Sprite pingSprite;
        public static Atlas.Sprite shipIconSprite;
        public static Config config = OptionsPanelHandler.Main.RegisterModOptions<Config>();
        public static TechType seaVoyagerFragmentTechType;

        public static Assembly executingAssembly = Assembly.GetExecutingAssembly();

        public static string ModFolderPath
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }
        public static string AssetsPath
        {
            get
            {
                return Path.Combine(ModFolderPath, "Assets");
            }
        }
        public static string AssetBundlePath
        {
            get
            {
                return Path.Combine(AssetsPath, "shipassets");
            }
        }

        public static float ShipMaxPower = 1000f;
        public static float ShipMaxPowerConsumptionRate = 12f;
        public static float ShipMaxPowerGenerationRate = 9f;

        public static Material glassMaterial;

        public static SeaVoyagerFragment seaVoyagerDockFragment;
        public static SeaVoyagerFragment seaVoyagerPoleFragment;
        public static SeaVoyagerFragment seaVoyagerHullFragment;
        public static SeaVoyagerFragment seaVoyagerLadderFragment;
        public static SeaVoyagerFragment seaVoyagerPropellerFragment;

        [QModPatch]
        public static void Patch()
        {
            string executingLocation = Path.GetDirectoryName(executingAssembly.Location);
            string folderPath = Path.Combine(executingLocation, "Assets");

            bundle = AssetBundle.LoadFromFile(AssetBundlePath);

            pingSprite = ImageUtils.LoadSpriteFromTexture(bundle.LoadAsset<Texture2D>("ShipPing"));
            SpriteHandler.RegisterSprite(SpriteManager.Group.Pings, "Alterra Sea Voyager", pingSprite);

            shipIconSprite = new Atlas.Sprite(bundle.LoadAsset<Sprite>("SeaVoyagerCraftIcon"));
            shipPingType = PingHandler.RegisterNewPingType("Alterra Sea Voyager", pingSprite);

            CoroutineHost.StartCoroutine(GetGlassMaterial());

            PatchMethods();
            PatchLanguageLines();
            PatchPrefabs();
            AddSpawns();
        }

        private static void PatchMethods()
        {
            //Manual patching is gross
            Harmony harmony = new Harmony("Lee23.SeaVoyager");
            var constructorOriginal = AccessTools.Method(typeof(ConstructorInput), "OnCraftingBegin");
            var constructorPrefix = new HarmonyMethod(AccessTools.Method(typeof(ConstructorInput_Patch), "Prefix"));
            harmony.Patch(constructorOriginal, constructorPrefix);

            var setInsideSeamothOG = AccessTools.Method(typeof(SeaMoth), "SetPlayerInside");
            var setInsideSeamothPost = new HarmonyMethod(AccessTools.Method(typeof(SetPlayerInside_Patch), "Postfix"));
            harmony.Patch(setInsideSeamothOG, null, setInsideSeamothPost);

            var onPlayerEnteredExosuitOG = AccessTools.Method(typeof(Exosuit), "OnPlayerEntered");
            var onPlayerEnteredExosuitPost = new HarmonyMethod(AccessTools.Method(typeof(EnterVehicle_Patch), "Postfix"));
            harmony.Patch(onPlayerEnteredExosuitOG, null, onPlayerEnteredExosuitPost);

            var spawnNearbyOriginal = AccessTools.Method(typeof(Player), "SpawnNearby");
            var spawnNearbyPostfix = new HarmonyMethod(AccessTools.Method(typeof(SpawnNearby_Patch), "Postfix"));
            harmony.Patch(spawnNearbyOriginal, null, spawnNearbyPostfix);

            var updateMoveOriginal = AccessTools.Method(typeof(GroundMotor), "Awake");
            var updateMovePrefix = new HarmonyMethod(AccessTools.Method(typeof(GroundMotor_Awake_Patch), "Postfix"));
            harmony.Patch(updateMoveOriginal, null, updateMovePrefix);

            var type = Type.GetType("SubnauticaMap.PingMapIcon, SubnauticaMap", false, false);
            if (type != null)
            {
                var pingOriginal = AccessTools.Method(type, "Refresh");
                var pingPrefix = new HarmonyMethod(AccessTools.Method(typeof(PingMapIcon_Patch), "Prefix"));
                harmony.Patch(pingOriginal, pingPrefix);
            }
        }

        private static void PatchLanguageLines()
        {
            LanguageHandler.SetLanguageLine("Ency_SeaVoyager", "Alterra Sea Voyager Piloting Manual");
            LanguageHandler.SetLanguageLine("EncyDesc_SeaVoyager", "A shallow-water alternative to the Cyclops submarine that provides extensive mobility and a large building space.\n\nTo control this vehicle, the pilot should select a speed and a movement direction. The ship will continue moving in the set direction. Pressing the off button will immediately decelerate the vessel to a halt. If you fall off of the deck of the Sea Voyager for any reason, the ship will automatically come to a stop.\n\nAdditional features:\n- Two above-water docks for small submersibles.\n- Front-facing camera to assist piloting through shallow waters.\n- Sonar map to examine the local environment.\n- Global positioning system that shows the vessel's location within the region.\n- Underwater window display in the Pilot's Cab to alleviate stress levels.\n\nPlease keep in mind this vehicle was not fully approved by any safety departments. As always, save often.");

            LanguageHandler.SetLanguageLine("SeaVoyagerAheadFlank", "Sea Voyager: Ahead flank. Emergency speed!");
            LanguageHandler.SetLanguageLine("SeaVoyagerAheadSlow", "Sea Voyager: Ahead slow.");
            LanguageHandler.SetLanguageLine("SeaVoyagerAheadStandard", "Sea Voyager: Ahead standard.");
            LanguageHandler.SetLanguageLine("SeaVoyagerShallowWater", "Sea Voyager: Attention! Approaching shallow water. Proceed with caution.");
            LanguageHandler.SetLanguageLine("SeaVoyagerPoweringDown", "Sea Voyager: Engine powering down.");
            LanguageHandler.SetLanguageLine("SeaVoyagerPoweringUp", "Sea Voyager: Engine powering up...");
            LanguageHandler.SetLanguageLine("SeaVoyagerFirstUse", "Sea Voyager MK. II operational and ready for service.");
            LanguageHandler.SetLanguageLine("SeaVoyagerNoPower", "Sea Voyager: Warning: Reserve power depleted.");
            LanguageHandler.SetLanguageLine("SeaVoyagerSonarMap", "Sea Voyager: Sonar map activated.");
            LanguageHandler.SetLanguageLine("SeaVoyagerRegionMap", "Sea Voyager: Updating regional map.");
            LanguageHandler.SetLanguageLine("SeaVoyagerVehicleAttached", "Sea Voyager: Vehicle attached.");
            LanguageHandler.SetLanguageLine("SeaVoyagerDockVehicle", "Sea Voyager: Vehicle docked successfully.");
            LanguageHandler.SetLanguageLine("SeaVoyagerVehicleReleased", "Sea Voyager: Vehicle released.");
            LanguageHandler.SetLanguageLine("SeaVoyagerWelcomeAboard", "Sea Voyager: Welcome aboard captain. All systems operational.");
        }

        private static void PatchPrefabs()
        {
            seaVoyagerFragmentTechType = TechTypeHandler.AddTechType(executingAssembly, "SeaVoyagerFragment", "Sea Voyager fragment", "A fragment of a destroyed Sea Voyager.", false);

            seaVoyagerPrefab = new ShipPrefab("SeaVoyager", "Alterra Sea Voyager", "A large self-sustaining ship that specializes in quick transportation.");
            seaVoyagerPrefab.Patch();

            PDAHandler.AddCustomScannerEntry(seaVoyagerFragmentTechType, seaVoyagerPrefab.TechType, true, "SeaVoyager", 6, 4f, true);

            seaVoyagerDockFragment = new SeaVoyagerFragment("SeaVoyagerFragment1", bundle.LoadAsset<GameObject>("SeaVoyagerFragment1"), 90f);
            seaVoyagerDockFragment.Patch();
            seaVoyagerPoleFragment = new SeaVoyagerFragment("SeaVoyagerFragment2", bundle.LoadAsset<GameObject>("SeaVoyagerFragment2"), 150f);
            seaVoyagerPoleFragment.Patch();
            seaVoyagerHullFragment = new SeaVoyagerFragment("SeaVoyagerFragment3", bundle.LoadAsset<GameObject>("SeaVoyagerFragment3"), 500f);
            seaVoyagerHullFragment.Patch();
            seaVoyagerLadderFragment = new SeaVoyagerFragment("SeaVoyagerFragment4", bundle.LoadAsset<GameObject>("SeaVoyagerFragment4"), 70f);
            seaVoyagerLadderFragment.Patch();
            seaVoyagerPropellerFragment = new SeaVoyagerFragment("SeaVoyagerFragment5", bundle.LoadAsset<GameObject>("SeaVoyagerFragment5"), 200f);
            seaVoyagerPropellerFragment.Patch();
        }

        private static void AddSpawns()
        {
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawns(new List<SpawnInfo>()
            {
                // Wreck 1
                new SpawnInfo(seaVoyagerHullFragment.TechType, new Vector3(93.21509f, -43.46548f, -6.106212f), new Vector3(3, 359, 338)),
                new SpawnInfo(seaVoyagerLadderFragment.TechType, new Vector3(104.19f, -45.66f, -0.48f), new Vector3(1, 0, 333)),
                new SpawnInfo(seaVoyagerPropellerFragment.TechType, new Vector3(92.58f, -39.59f, 7.39f), new Vector3(28, 0, 137)),
                new SpawnInfo(seaVoyagerDockFragment.TechType, new Vector3(96.32f, -51.44f, -17.64f), new Vector3(0, 0, 189)),

                // Wreck 2
                new SpawnInfo(seaVoyagerHullFragment.TechType, new Vector3(-71.41f, -35.79f, 416f), new Vector3(0, 34, 0)),
                new SpawnInfo(seaVoyagerDockFragment.TechType, new Vector3(-72.00f, -35.20f, 402.00f), new Vector3(17, 69, 103)),
                new SpawnInfo(seaVoyagerDockFragment.TechType, new Vector3(-62.54f, -27.83f, 429.26f), new Vector3(0, 358, 15)),
                new SpawnInfo(seaVoyagerPropellerFragment.TechType, new Vector3(-68.14f, -37.00f, 404.32f), new Vector3(30, 232, 180)),
                new SpawnInfo(seaVoyagerPoleFragment.TechType, new Vector3(-50.17f, -23.18f, 423.00f), new Vector3(279, 193, 180)),
                new SpawnInfo(seaVoyagerLadderFragment.TechType, new Vector3(-56.70f, -26.63f, 426.50f), new Vector3(347, 359, 12)),
                new SpawnInfo(seaVoyagerLadderFragment.TechType, new Vector3(-45.51f, -24.08f, 411.84f), new Vector3(2, 0, 358)),
            });
        }

        private static IEnumerator GetGlassMaterial()
        {
            var task = CraftData.GetPrefabForTechTypeAsync(TechType.Aquarium);
            yield return task;
            var reference = task.GetResult();

            Renderer[] renderers = reference.GetComponentsInChildren<Renderer>(true);

            foreach (Renderer renderer in renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    if (material.name.ToLower().Contains("glass"))
                    {
                        glassMaterial = material;
                        yield break;
                    }
                }
            }
        }
        
        public static void PrintExoCustomControls()
        {
            ErrorMessage.AddMessage(string.Format("Return to surface: '{0}'", new[] { GameInput.GetBindingName(GameInput.Button.MoveUp, GameInput.BindingSet.Primary) }));
            ErrorMessage.AddMessage(string.Format("Descend: '{0}'", new[] { GameInput.GetBindingName(GameInput.Button.MoveDown, GameInput.BindingSet.Primary) }));
            ErrorMessage.AddMessage(string.Format("Detach cable: '{0}'", new[] { GameInput.GetBindingName(GameInput.Button.Deconstruct, GameInput.BindingSet.Primary) }));
        }
        
        //Patch the builidng of constructor input
        public static class ConstructorInput_Patch
        {
            public static void Prefix(TechType techType, ref float duration)
            {
                if(techType == seaVoyagerPrefab.TechType)
                {
                    duration = 20f; //Takes 20 seconds to build
                    FMODUWE.PlayOneShot("event:/tools/constructor/spawn", Player.main.transform.position, 1f); //Cyclops does this i think
                }
            }
        }

        public static class GroundMotor_Awake_Patch
        {
            public static void Postfix(GroundMotor __instance)
            {
                __instance.movingPlatform.movementTransfer = GroundMotor.MovementTransferOnJump.PermaLocked;
            }
        }

        public static class SetPlayerInside_Patch
        {
            public static void Postfix(SeaMoth __instance, bool inside)
            {
                if (inside)
                {
                    var heldByCable = __instance.gameObject.GetComponent<HeldByCable>();
                    if (heldByCable != null)
                    {
                        if (heldByCable.dock != null)
                        {
                            PrintExoCustomControls();
                        }
                    }
                }
            }
        }

        public static class EnterVehicle_Patch
        {
            public static void Postfix(Exosuit __instance)
            {
                    var heldByCable = __instance.gameObject.GetComponent<HeldByCable>();
                    if (heldByCable != null)
                    {
                        if (heldByCable.dock != null)
                        {
                            PrintExoCustomControls();
                        }
                    }
            }
        }

        public static class SpawnNearby_Patch
        {
            public static void Postfix(GameObject ignoreObject, ref bool __result)
            {
                var vehicle = ignoreObject.GetComponent<Vehicle>();
                if (vehicle != null)
                {
                    if (Player.main.GetVehicle() != vehicle)
                    {
                        return;
                    }
                    var hbc = vehicle.GetComponent<HeldByCable>();
                    if (hbc)
                    {
                        if (hbc.Docked)
                        {
                            if(Vector3.Distance(hbc.dock.transform.position, hbc.transform.position) > 15f)
                            {
                                return;
                            }
                            __result = true;
                            Player.main.SetPosition(hbc.dock.ship.gameObject.FindChild("ExosuitExit").transform.position);
                            Player.main.transform.parent = null;
                        }
                    }
                }
            }
        }

        public static class PingMapIcon_Patch
        {
            [HarmonyPrefix]
            public static bool Prefix(object __instance)
            {
                FieldInfo field = __instance.GetType().GetField("ping");
                PingInstance ping = field.GetValue(__instance) as PingInstance;
                if(ping.pingType == shipPingType)
                {
                    FieldInfo field2 = __instance.GetType().GetField("icon");
                    uGUI_Icon icon = field2.GetValue(__instance) as uGUI_Icon;
                    icon.sprite = SpriteManager.Get(SpriteManager.Group.Pings, "Alterra Sea Voyager");
                    icon.color = Color.black;
                    RectTransform rectTransform = icon.rectTransform;
                    rectTransform.sizeDelta = Vector2.one * 28f;
                    rectTransform.localPosition = Vector3.zero;
                    return false;
                }
                return true;
            }
        }
    }
}
