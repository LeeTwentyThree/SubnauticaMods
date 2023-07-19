using HarmonyLib;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;

namespace AggressiveFauna.Patchers;

[HarmonyPatch(typeof(AggressiveWhenSeeTarget))]
[HarmonyPatch("GetAggressionTarget")]
internal class Aggression_GetAggressionTarget_Patch
{
    [HarmonyPrefix]
    public static bool Prefix(AggressiveWhenSeeTarget __instance)
    {
        return false;
    }

    [HarmonyPostfix]
    public static void Postfix(AggressiveWhenSeeTarget __instance, ref GameObject __result)
    {
        int maxSearchRings = __instance.maxSearchRings;

        maxSearchRings *= AggressionSettings.SearchRingScale;

        /*if (Ocean.main.GetDepthOf(Player.main.gameObject) <= 5)
        {
            if (maxSearchRings > __instance.maxSearchRings + 1) maxSearchRings = __instance.maxSearchRings + 1;
        }*/

        IEcoTarget ecoTarget = EcoRegionManager.main.FindNearestTarget(__instance.targetType, __instance.transform.position, __instance.isTargetValidFilter, maxSearchRings);
        if (ecoTarget == null)
        {
            __result = null;
        }
        else
        {
            __result = ecoTarget.GetGameObject();
        }
    }
}

[HarmonyPatch(typeof(Creature), nameof(Creature.IsInFieldOfView))]
internal class Creature_IsInFieldOfView_Patch
{
    [HarmonyPrefix]
    public static bool Prefix(Creature __instance, GameObject go, ref bool __result)
    {
        __result = false;
        if (go != null)
        {
            Vector3 vector = go.transform.position - __instance.transform.position;
            Vector3 rhs = __instance.eyesOnTop ? __instance.transform.up : __instance.transform.forward;
            Vector3 normalized = vector.normalized;
            var dot = Vector3.Dot(normalized, rhs);
            var fovMult = AggressionSettings.FOVMultiplier;
            if (dot >= 0f) dot *= fovMult;
            else if (fovMult > 0f) dot /= fovMult;
            if ((Mathf.Approximately(__instance.eyeFOV, -1f) || dot >= __instance.eyeFOV))
            {
                if (AggressionSettings.CanSeeThroughTerrain || !Physics.Linecast(__instance.transform.position, go.transform.position, Voxeland.GetTerrainLayerMask()))
                {
                    __result = true;
                }
            }
        }
        return false;
    }
}

[HarmonyPatch(typeof(SwimBehaviour), nameof(SwimBehaviour.Attack))]
internal class SwimBehaviour_Attack_Patch
{
    [HarmonyPrefix]
    public static bool Prefix(SwimBehaviour __instance, Vector3 targetPosition, Vector3 targetDirection, float velocity)
    {
        __instance.SwimToInternal(targetPosition, targetDirection, velocity * AggressionSettings.AttackChargeVelocityScale, true, false);
        return false;
    }
}

[HarmonyPatch(typeof(MeleeAttack), nameof(MeleeAttack.CanBite))]
internal class MeleeAttack_CanBite_Patch
{
    [HarmonyPrefix]
    public static bool Prefix(MeleeAttack __instance, GameObject target, ref bool __result)
    {
        Player player = target.GetComponent<Player>();
        if (__instance.frozen)
        {
            __result = false;
            return false;
        }
        if (player != null && !player.CanBeAttacked())
        {
            __result = false;
            return false;
        }
        if (__instance.creature.Aggression.Value * AggressionSettings.AggressionMultiplier < __instance.biteAggressionThreshold)
        {
            __result = false;
            return false;
        }
        if (Time.time < __instance.timeLastBite + __instance.biteInterval * AggressionSettings.BiteCooldownScale)
        {
            __result = false;
            return false;
        }
        bool isCyclops = target.GetComponent<SubControl>() != null;
        if (isCyclops && target != __instance.lastTarget.target)
        {
            __result = false;
            return false;
        }
        if ((!__instance.canBitePlayer || player == null) &&
            (!__instance.canBiteCreature || target.GetComponent<Creature>() == null) &&
            ((!AggressionSettings.AlwaysBiteVehicles && !__instance.canBiteVehicle) || target.GetComponent<Vehicle>() == null) &&
            ((!AggressionSettings.AlwaysBiteCyclops && !__instance.canBiteCyclops) || (!isCyclops && target.GetComponent<CyclopsDecoy>() == null)))
        {
            __result = false;
            return false;
        }
        Vector3 direction = target.transform.position - __instance.transform.position;
        float magnitude = direction.magnitude;
        int num = UWE.Utils.RaycastIntoSharedBuffer(__instance.transform.position, direction, magnitude, -5, QueryTriggerInteraction.Ignore);
        for (int i = 0; i < num; i++)
        {
            Collider collider = UWE.Utils.sharedHitBuffer[i].collider;
            GameObject gameObject = (collider.attachedRigidbody != null) ? collider.attachedRigidbody.gameObject : collider.gameObject;
            if (!(gameObject == target) && !(gameObject == __instance.gameObject) && !(gameObject.GetComponent<Creature>() != null))
            {
                __result = false;
                return false;
            }
        }
        __result = true;
        return false;
    }
}

[HarmonyPatch(typeof(AttackLastTarget), nameof(AttackLastTarget.Evaluate))]
internal class AttackLastTarget_AttackLastTarget_Patch
{
    [HarmonyPrefix]
    public static bool Prefix(AttackLastTarget __instance, Creature creature, ref float __result)
    {
        if ((creature.Aggression.Value * AggressionSettings.AggressionMultiplier > __instance.aggressionThreshold || Time.time < __instance.timeStartAttack + __instance.minAttackDuration * AggressionSettings.AttackDurationScale)
            && Time.time > __instance.timeStopAttack + __instance.pauseInterval * AggressionSettings.AttackCooldownScale)
        {
            if (__instance.lastTarget.target != null && Time.time <= __instance.lastTarget.targetTime + __instance.rememberTargetTime * AggressionSettings.RememberTargetTimeScale && !__instance.lastTarget.targetLocked)
            {
                __instance.currentTarget = __instance.lastTarget.target;
            }
            if (!__instance.CanAttackTarget(__instance.currentTarget))
            {
                __instance.currentTarget = null;
            }
            if (__instance.currentTarget != null)
            {
                __result = __instance.GetEvaluatePriority();
            }
            return false;
        }
        __result = 0f;
        return false;
    }
}

[HarmonyPatch(typeof(AttackLastTarget), nameof(AttackLastTarget.CanAttackTarget))]
internal class AttackLastTarget_CanAttackTarget_Patch
{
    [HarmonyPrefix]
    public static bool Prefix(AttackLastTarget __instance, GameObject target, ref bool __result)
    {
        if (target == null)
        {
            __result = false;
            return false;
        }
        LiveMixin lm = target.GetComponent<LiveMixin>();
        __result = lm && lm.IsAlive() && (!(target == Player.main.gameObject) || AggressionUtils.PlayerCanBeTargeted(Player.main));
        return false;
    }
}

[HarmonyPatch(typeof(ReaperMeleeAttack), nameof(ReaperMeleeAttack.OnTouch))]
internal class ReaperMeleeAttack_OnTouch_Patch
{
    [HarmonyPrefix]
    public static void Prefix(ReaperMeleeAttack __instance)
    {
        if (__instance.creature.Aggression.Value * AggressionSettings.AggressionMultiplier >= 0.5f)
        {
            __instance.creature.Aggression.Value = 0.5f; // hacky way to scale up the reaper's aggression so he will always be able to bite
        }
    }
}

[HarmonyPatch(typeof(AggressiveWhenSeeTarget), "IsTargetValid", new Type[] { typeof(GameObject) })]
internal class Aggression_IsTargetValid_Patch
{
    [HarmonyPrefix]
    public static bool Prefix(GameObject target, AggressiveWhenSeeTarget __instance)
    {
        return false;
    }

    [HarmonyPostfix]
    public static void Postfix(GameObject target, AggressiveWhenSeeTarget __instance, ref bool __result)
    {
        if (target == null)
        {
            __result = false;
            return;
        }
        if (AggressionSettings.AllowFriends && __instance.creature.GetFriend() == target)
        {
            __result = false;
            return;
        }
        if (target == Player.main.gameObject)
        {
            if (!AggressionUtils.PlayerCanBeTargeted(Player.main))
            {
                __result = false;
                return;
            }
        }
        if (__instance.ignoreSameKind && CraftData.GetTechType(target) == __instance.myTechType)
        {
            __result = false;
            return;
        }
        if (__instance.targetShouldBeInfected)
        {
            InfectedMixin component = target.GetComponent<InfectedMixin>();
            if (component == null || component.GetInfectedAmount() < 0.33f)
            {
                __result = false;
                return;
            }
        }

        float dist = Vector3.Distance(target.transform.position, __instance.transform.position);
        if (dist > __instance.maxRangeScalar)
        {
            if (((target != Player.main.gameObject) && !target.GetComponent<Vehicle>()) // if not the player
                || (dist > __instance.maxRangeScalar * AggressionSettings.MaxDistanceScale)) // or if you ARE the player and are not in the scaled range, fail
            {
                __result = false;
                return;
            }

            if (target == Player.main.gameObject) // if it's the player, don't detect him while he's in a precursor base
            {
                if (Player.main.precursorOutOfWater || PrecursorMoonPoolTrigger.inMoonpool)
                {
                    __result = false;
                    return;
                }
            }

            if (target.GetComponent<Vehicle>() != null)
            { // don't attack abandoned vehicles, that's just annoying
                if (((target.GetComponent<Vehicle>() != Player.main.currentMountedVehicle) && !AggressionSettings.AttackEmptyVehicles) || target.GetComponent<Vehicle>().precursorOutOfWater || PrecursorMoonPoolTrigger.inMoonpool)
                {
                    __result = false;
                    return;
                }
            }

        }
        if (!Mathf.Approximately(__instance.minimumVelocity, 0f))
        {
            Rigidbody componentInChildren = target.GetComponentInChildren<Rigidbody>();
            if (componentInChildren != null && componentInChildren.velocity.magnitude <= __instance.minimumVelocity)
            {
                __result = false;
                return;
            }
        }

        if ((((target != Player.main.gameObject) || (!AggressionSettings.CanSeeInsideBases && Player.main.IsInside()) || Player.main.precursorOutOfWater || PrecursorMoonPoolTrigger.inMoonpool) && (!target.GetComponent<Vehicle>() || (target.GetComponent<Vehicle>() != Player.main.currentMountedVehicle) || target.GetComponent<Vehicle>().precursorOutOfWater)) ||  // Must be player or vehicle
            (Ocean.GetDepthOf(target) <= 5))                                    // Keeps reapers from eating us up on land
        {
            __result = __instance.creature.GetCanSeeObject(target);
        }
        else
        {
            if (AggressionSettings.CanSeeThroughTerrain)
            {
                __result = true;
            }
            else
            {
                __result = !Physics.Linecast(__instance.transform.position, target.transform.position, Voxeland.GetTerrainLayerMask());
            }
        }
    }
}


[HarmonyPatch(typeof(EcoRegion))]
[HarmonyPatch("FindNearestTarget")]
internal class EcoRegion_FindNearestTarget_Patch
{
    [HarmonyPrefix]
    public static bool PreFix(EcoTargetType type, Vector3 wsPos, EcoRegion.TargetFilter isTargetValid, ref float bestDist, ref IEcoTarget best)
    {
        return false;
    }

    [HarmonyPostfix]
    public static void PostFix(EcoRegion __instance, EcoTargetType type, Vector3 wsPos, EcoRegion.TargetFilter isTargetValid, ref float bestDist, ref IEcoTarget best)
    {
        __instance.timeStamp = Time.time;
        HashSet<IEcoTarget> hashSet;
        if (!__instance.ecoTargets.TryGetValue((int)type, out hashSet))
        {
            ProfilingUtils.EndSample(null);
            return;
        }
        float num = float.MaxValue;
        foreach (IEcoTarget ecoTarget in hashSet) // iterate over each eco target
        {
            if (ecoTarget != null && !ecoTarget.Equals(null)) // make sure it exists still
            {
                float sqrMagnitude = (wsPos - ecoTarget.GetPosition()).sqrMagnitude;

                // if we're looking at the player
                if (((ecoTarget.GetGameObject() == Player.main.gameObject) && !Player.main.IsInside() && Player.main.IsUnderwater() && !Player.main.precursorOutOfWater) ||
                    (ecoTarget.GetGameObject().GetComponent<Vehicle>() && (ecoTarget.GetGameObject().GetComponent<Vehicle>() == Player.main.currentMountedVehicle)) && !Player.main.currentMountedVehicle.precursorOutOfWater)
                {
                    bool feeding = false;
                    if (AggressionSettings.CanFeed && ecoTarget.GetGameObject() == Player.main.gameObject)
                    {
                        Pickupable held = Inventory.main.GetHeld();
                        if (held != null && (held.GetTechType() == TechType.Peeper))
                        {
                            feeding = true;
                        }
                    }

                    float depth = Ocean.GetDepthOf(ecoTarget.GetGameObject());
                    if ((depth > 5) && !feeding) // force the player to be targeted ny making him the closest target always
                    {
                        sqrMagnitude /= AggressionSettings.PlayerPrioritizationMultiplier;
                    }
                }

                if (sqrMagnitude < num && (isTargetValid == null || isTargetValid(ecoTarget)))
                {
                    best = ecoTarget;
                    num = sqrMagnitude;
                }
            }
        }
        if (best != null)
        {
            bestDist = Mathf.Sqrt(num);
        }
    }
}

// Increases aggression and search radius when attacking
[HarmonyPatch(typeof(MoveTowardsTarget))]
[HarmonyPatch("UpdateCurrentTarget")]
internal class MoveTowardsTarget_UpdateCurrentTarget_Patch
{
    [HarmonyPrefix]
    public static bool Prefix(MoveTowardsTarget __instance)
    {
        if (CraftData.GetTechType(__instance.gameObject) == TechType.Crash)
        {
            return true; // Explody ambush fish just runs normal method
        }

        if (Player.main.precursorOutOfWater || !Player.main.IsUnderwater() || PrecursorMoonPoolTrigger.inMoonpool || (Ocean.GetDepthOf(Player.main.gameObject) < 5))
        {
            return true;
        }

        Vehicle veh = Player.main.currentMountedVehicle;
        if (veh != null)
        {
            if (veh.precursorOutOfWater) return true;
        }

        float aggressionMultiplier = AggressionSettings.AggressionMultiplier;
        int aggressionRadius = AggressionSettings.SearchRingScale;

        if (EcoRegionManager.main != null && (Mathf.Approximately(__instance.requiredAggression, 0f) || __instance.creature.Aggression.Value * aggressionMultiplier >= __instance.requiredAggression))
        {
            IEcoTarget ecoTarget = EcoRegionManager.main.FindNearestTarget(__instance.targetType, __instance.transform.position, __instance.isTargetValidFilter, aggressionRadius);

            if (ecoTarget != null)
            {
                __instance.currentTarget = ecoTarget;
            }
            else
            {
                __instance.currentTarget = null;
            }
        }
        return false;
    }
}


