using HarmonyLib;
using UnityEngine;

namespace SeaVoyager.Patches;

[HarmonyPatch]
public static class FixJumpBugPatch
{
    private static readonly GroundMotor.MovementTransferOnJump InsideSeaVoyagerMode =
        GroundMotor.MovementTransferOnJump.PermaLocked;
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SubRoot))]
    [HarmonyPatch(nameof(SubRoot.OnPlayerEntered))]
    public static void OnEnter(SubRoot __instance)
    {
        if (!IsSeaVoyager(__instance))
        {
            return;
        }
        var platform = GetMovingPlatform();
        if (!RememberPlayerState.HasRememberedOriginalMode())
        {
            RememberPlayerState.RememberMovementTransferMode(platform.movementTransfer);
        }

        platform.movementTransfer = InsideSeaVoyagerMode;
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SubRoot))]
    [HarmonyPatch(nameof(SubRoot.OnPlayerExited))]
    public static void OnExit(SubRoot __instance)
    {
        if (!IsSeaVoyager(__instance)) return;
        if (RememberPlayerState.TryGetRememberedMode(out var lastMode))
        {
            GetMovingPlatform().movementTransfer = lastMode;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GroundMotor))]
    [HarmonyPatch(nameof(GroundMotor.Awake))]
    public static void GroundMotorStartPostfix(GroundMotor __instance)
    {
        if (Plugin.config.OverrideGameMovement)
        {
            __instance.movingPlatform.movementTransfer = InsideSeaVoyagerMode;
        }
    }

    private static GroundMotor.CharacterMotorMovingPlatform GetMovingPlatform()
    {
        return Player.main.groundMotor.movingPlatform;
    }

    private static bool IsSeaVoyager(SubRoot sub)
    {
        return sub is Mono.SeaVoyager;
    }

    private class RememberPlayerState : MonoBehaviour
    {
        private static RememberPlayerState Main { get; set; }

        private GroundMotor.MovementTransferOnJump _transfer;
        
        private void Awake()
        {
            Main = this;
        }

        public static bool HasRememberedOriginalMode()
        {
            return Main != null;
        }

        public static void RememberMovementTransferMode(GroundMotor.MovementTransferOnJump mode)
        {
            if (Main == null)
            {
                Main = new GameObject("SeaVoyagerRememberPlayerState").AddComponent<RememberPlayerState>();
            }

            Main._transfer = mode;
        }

        public static bool TryGetRememberedMode(out GroundMotor.MovementTransferOnJump mode)
        {
            if (Main == null)
            {
                mode = default;
                return false;
            }

            mode = Main._transfer;
            return true;
        }
    }
}