using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using QModManager.API.ModLoading;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Utility;
using UnityEngine;
using ShipMod.Ship;
using HarmonyLib;
using SMLHelper.V2.Handlers;

namespace ShipMod
{
    [QModCore]
    public static class QPatch
    {
        public static AssetBundle bundle;
        public static ShipPrefab shipPrefab;
        public static PingType shipPingType;

        public static Atlas.Sprite pingSprite;
        public static Atlas.Sprite shipIconSprite;
        public static FMODAsset welcomeSoundAsset;
        public static FMODAsset noPowerSoundAsset;
        public static FMODAsset powerDownSoundAsset;
        public static Config config = OptionsPanelHandler.Main.RegisterModOptions<Config>();

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

        [QModPatch]
        public static void Patch()
        {
            string executingLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string folderPath = Path.Combine(executingLocation, "Assets");

            bundle = AssetBundle.LoadFromFile(AssetBundlePath);

            pingSprite = ImageUtils.LoadSpriteFromTexture(bundle.LoadAsset<Texture2D>("ship_ping.png"));
            SpriteHandler.RegisterSprite(SpriteManager.Group.Pings, "Alterra Sea Voyager", pingSprite);

            shipIconSprite = ImageUtils.LoadSpriteFromTexture(bundle.LoadAsset<Texture2D>("ship_icon.png"));
            shipPingType = PingHandler.RegisterNewPingType("Alterra Sea Voyager", pingSprite);

            welcomeSoundAsset = ScriptableObject.CreateInstance<FMODAsset>();
            welcomeSoundAsset.id = "shipwelcome";
            welcomeSoundAsset.path = "event:/sub/cyclops/AI_welcome";
            noPowerSoundAsset = ScriptableObject.CreateInstance<FMODAsset>();
            noPowerSoundAsset.id = "shipnopower";
            noPowerSoundAsset.path = "event:/sub/cyclops/AI_no_power";
            powerDownSoundAsset = ScriptableObject.CreateInstance<FMODAsset>();
            powerDownSoundAsset.id = "shippowerdown";
            powerDownSoundAsset.path = "event:/sub/cyclops/AI_engine_down";

            shipPrefab = new ShipPrefab("SeaVoyager", "Alterra Sea Voyager", "A large self-sustaining ship that specializes in quick transportation.");
            shipPrefab.Patch();

            //Manual patching is gross
            Harmony harmony = new Harmony("Lee23.ShipMod");
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

            LanguageHandler.SetLanguageLine("Ency_SeaVoyager", "Alterra Sea Voyager Piloting Manual");
            LanguageHandler.SetLanguageLine("EncyDesc_SeaVoyager", "A shallow-water alternative to the Cyclops submarine that provides extensive mobility and a large building space.\n\nPiloting this vehicle is simple. Simply choose a direction and it will keep going in that direction. Pressing the off button will immediately decelerate the vessel. In an emergency, the vehicle will automatically shut down. You may also use the down-facing camera or map to aid in piloting.\n\nPlease keep in mind this vehicle was not approved by any safety departments. As always, save often.");

            var type = Type.GetType("SubnauticaMap.PingMapIcon, SubnauticaMap", false, false);
            if (type != null)
            {
                var pingOriginal = AccessTools.Method(type, "Refresh");
                var pingPrefix = new HarmonyMethod(AccessTools.Method(typeof(PingMapIcon_Patch), "Prefix"));
                harmony.Patch(pingOriginal, pingPrefix);
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
                if(techType == shipPrefab.TechType)
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
