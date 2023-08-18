using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using System.Reflection.Emit;

namespace CustomWaterLevel;

internal static class Patches
{
    [HarmonyPatch(typeof(Ocean))]
    internal static class Ocean_Patches
    {
        [HarmonyPatch(nameof(Ocean.Awake))]
        [HarmonyPostfix]
        public static void Awake_Postfix(Ocean __instance)
        {
            __instance.defaultOceanLevel = Plugin.WaterLevel;

            UpdateOceanPosition();

            __instance.gameObject.EnsureComponent<OceanHelper>().ocean = __instance;
        }

        [HarmonyPatch(nameof(Ocean.RestoreOceanLevel))]
        [HarmonyPostfix]
        public static void RestoreOceanLevel_Postfix()
        {
            UpdateOceanPosition();
        }

        private static void UpdateOceanPosition()
        {
            var pos = Ocean.main.transform.position;
            pos.y = Plugin.WaterLevel;
            Ocean.main.transform.position = pos;
        }
    }

    [HarmonyPatch(typeof(WaterPlane))]
    [HarmonyPatch(nameof(WaterPlane.Start))]
    internal static class WaterPlane_Start_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(WaterPlane __instance)
        {
            __instance.transform.position = new Vector3(0f, Plugin.WaterLevel, 0f);
            __instance.gameObject.EnsureComponent<SetYToWaterLevel>();
        }
    }

    [HarmonyPatch(typeof(WaterSurface))]
    [HarmonyPatch(nameof(WaterSurface.Start))]
    internal static class WaterSurface_Start_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(WaterSurface __instance)
        {
            __instance.transform.position = new Vector3(0f, Plugin.WaterLevel, 0f);
            __instance.gameObject.EnsureComponent<SetYToWaterLevel>();
        }
    }

    [HarmonyPatch(typeof(EscapePod))]
    [HarmonyPatch(nameof(EscapePod.Start))]
    internal static class EscapePod_Start_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(EscapePod __instance)
        {
            __instance.gameObject.EnsureComponent<FreezeLifepodWhenFar>();
        }
    }

    [HarmonyPatch(typeof(WorldForces))]
    [HarmonyPatch(nameof(WorldForces.Start))]
    internal static class WorldForces_Awake_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(WorldForces __instance)
        {
            __instance.gameObject.EnsureComponent<WorldForcesHelper>().worldForces = __instance;
        }
    }

    [HarmonyPatch(typeof(BaseSurfaceModel))]
    [HarmonyPatch("IBaseGhostModel.BuildModel")]
    internal static class BaseSurfaceModel_BuildModel_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var foundIndex = -1;

            var codes = new List<CodeInstruction>(instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldc_R4)
                {
                    if (codes[i].OperandIs(1f))
                    {
                        foundIndex = i;
                        break;
                    }
                }
            }
            if (foundIndex > -1)
            {
                EditCodeInstruction(codes, foundIndex, AccessTools.Method(typeof(Patches), nameof(GetLandDoorWaterLevel)));
            }

            return codes.AsEnumerable();

        }
    }

    [HarmonyPatch(typeof(Drowning))]
    [HarmonyPatch(nameof(Drowning.Update))]
    internal static class Drowning_Update_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(Drowning __instance)
        {
            if (__instance.gameObject.transform.position.y < Plugin.WaterLevel)
            {
                return true;
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(VFXSchoolFish))]
    [HarmonyPatch(nameof(VFXSchoolFish.Awake))]
    internal static class VFXSchoolFish_Awake_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(VFXSchoolFish __instance)
        {
            bool shouldRemove = __instance.transform.position.y > Plugin.WaterLevel - 5f;
            if (Plugin.config.RemoveSchoolsOfFish)
            {
                shouldRemove = true;
            }
            if (shouldRemove)
            {
                Object.Destroy(__instance.gameObject);
            }
        }
    }

    [HarmonyPatch(typeof(Creature))]
    [HarmonyPatch(nameof(Creature.Start))]
    internal static class Creature_Start_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Creature __instance)
        {
            if (__instance.liveMixin == null)
            {
                return;
            }
            if (Plugin.config.SuffocateFish)
            {
                var creatureTechType = CraftData.GetTechType(__instance.gameObject);
                if (Plugin.NonSuffocatingCreatures.Contains(creatureTechType))
                {
                    return;
                }
                var waterParkCreature = __instance.gameObject.GetComponent<WaterParkCreature>();
                if (waterParkCreature != null)
                {
                    if (waterParkCreature.IsInsideWaterPark())
                    {
                        return;
                    }
                }
                __instance.gameObject.AddComponent<CreatureSuffocator>().creature = __instance;
            }
        }
    }

    [HarmonyPatch(typeof(Exosuit))]
    internal static class Exosuit_Patches
    {
        [HarmonyPatch(nameof(Exosuit.IsUnderwater))]
        [HarmonyPostfix]
        public static void IsUnderwater_Postfix(ref bool __result)
        {
            if (Plugin.config.BuffExosuit)
            {
                __result = true;
            }
        }
    }

    [HarmonyPatch(typeof(Constructable))]
    internal static class Constructable_Patches
    {
        [HarmonyPatch(nameof(Constructable.CheckFlags))]
        [HarmonyPostfix]
        public static void CheckFlags_Prefix(ref bool __result, bool allowedInBase, bool allowedInSub, bool allowedOutside, bool allowedUnderwater, Vector3 hitPoint)
        {
            if (Player.main.GetCurrentSub() != null || !allowedOutside)
            {
                return;
            }
            else
            {
                if (!allowedUnderwater && hitPoint.y > Plugin.WaterLevel)
                {
                    __result = true;
                }
            }
        }
    }

    [HarmonyPatch(typeof(UnderwaterMotor))]
    [HarmonyPatch(nameof(UnderwaterMotor.UpdateMove))]
    public static class UnderwaterMotor_UpdateMove_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var foundIndex = -1;

            var codes = new List<CodeInstruction>(instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldc_R4)
                {
                    if (codes[i].OperandIs(-0.5f))
                    {
                        foundIndex = i;
                        break;
                    }
                }
            }
            if (foundIndex > -1)
            {
                EditCodeInstruction(codes, foundIndex, AccessTools.Method(typeof(Patches), nameof(GetUpdateMoveWaterLevel)));
            }

            return codes.AsEnumerable();

        }
    }

    public static float GetUpdateMoveWaterLevel()
    {
        return Plugin.WaterLevel - 0.5f;
    }

    public static float GetSeaTruckWaterLevel()
    {
        return Plugin.WaterLevel - 1.5f;
    }

    public static float GetLandDoorWaterLevel()
    {
        return Plugin.WaterLevel + 1f;
    }

    public static float GetWaterLevel()
    {
        return Plugin.WaterLevel;
    }

    public static void EditCodeInstruction(List<CodeInstruction> list, int index, MethodInfo targetMethod)
    {
        list[index] = new CodeInstruction(OpCodes.Call, targetMethod);
    }

    [HarmonyPatch(typeof(UnderWaterTracker))]
    [HarmonyPatch(nameof(UnderWaterTracker.UpdateWaterState))]
    public static class UnderWaterTracker_UpdateWaterState_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var foundIndex = -1;

            var codes = new List<CodeInstruction>(instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldc_R4)
                {
                    if (codes[i].OperandIs(0.0f))
                    {
                        foundIndex = i;
                        break;
                    }
                }
            }
            if (foundIndex > -1)
            {
                EditCodeInstruction(codes, foundIndex, AccessTools.Method(typeof(Patches), nameof(GetWaterLevel)));
            }

            return codes.AsEnumerable();

        }
    }

    [HarmonyPatch(typeof(Locomotion))]
    [HarmonyPatch(nameof(Locomotion.ManagedFixedUpdate))]
    public static class Locomotion_ManagedFixedUpdate_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var foundIndex = -1;

            var codes = new List<CodeInstruction>(instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldc_R4)
                {
                    if (codes[i].OperandIs(0.0f))
                    {
                        foundIndex = i;
                        break;
                    }
                }
            }
            if (foundIndex > -1)
            {
                EditCodeInstruction(codes, foundIndex, AccessTools.Method(typeof(Patches), nameof(GetWaterLevel)));
            }

            return codes.AsEnumerable();

        }
    }

    [HarmonyPatch(typeof(BaseGhost))]
    [HarmonyPatch(nameof(BaseGhost.PlaceWithBoundsCast))]
    internal static class BaseGhost_PlaceWithBoundsCast_Patch
    {
        [HarmonyPrefix]
        public static void Prefix(BaseGhost __instance)
        {
            __instance.allowedAboveWater = true;
        }
    }

    [HarmonyPatch(typeof(VFXConstructing))]
    [HarmonyPatch(nameof(VFXConstructing.Awake))]
    internal static class VFXConstructing_Awake_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(VFXConstructing __instance)
        {
            __instance.gameObject.EnsureComponent<VFXConstructionHelper>().constructing = __instance;
        }
    }

    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch(nameof(Player.Start))]
    internal static class Player_Start_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Player __instance)
        {
            if (Plugin.config.AutomaticChange)
            {
                __instance.gameObject.EnsureComponent<WaterMove>();
            }
        }
    }

    [HarmonyPatch(typeof(Constructor))]
    [HarmonyPatch(nameof(Constructor.OnRightHandDown))]
    internal static class Constructor_OnRightHandDown_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            if (Plugin.config.FixConstructor)
            {
                return false; // skip original method
            }
            return true;
        }

        [HarmonyPostfix]
        public static void Postfix(Constructor __instance, ref bool __result)
        {
            if (!Plugin.config.FixConstructor)
            {
                return;
            }
            if (__result == true)
            {
                return; // if we already deployed it, we don't need to deploy a second MBV!
            }
            if (PrecursorMoonPoolTrigger.inMoonpool || PrisonManager.IsInsideAquarium(__instance.transform.position) || Player.main.IsInSub())
            {
                ErrorMessage.AddMessage("Can't deploy here!");
                return; // don't need to throw it if you're inside a precursor base, or your own base
            }
            // I nabbed this from dnSpy, that's why it looks so weird
            Vector3 forward = MainCamera.camera.transform.forward;
            __instance.pickupable.Drop(__instance.transform.position + forward * 0.7f + Vector3.down * 0.3f, default(Vector3));
            __instance.GetComponent<Rigidbody>().AddForce(forward * 6.5f, ForceMode.VelocityChange);
            __instance.Deploy(true);
            __instance.OnDeployAnimationStart();
            LargeWorldEntity.Register(__instance.gameObject);
            Utils.PlayEnvSound(__instance.releaseSound, MainCamera.camera.transform.position, 20f);
            GoalManager.main.OnCustomGoalEvent("Release_Constructor");
            __result = true; // play the animation
        }
    }
}
