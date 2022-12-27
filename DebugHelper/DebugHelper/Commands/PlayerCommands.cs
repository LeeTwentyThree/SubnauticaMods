using SMLHelper.V2.Commands;
using UnityEngine;

namespace DebugHelper.Commands
{
    public static class PlayerCommands
    {
        [ConsoleCommand("forcewalkmode")]
        public static void ForceWalkMode()
        {
            Player.main.SetPrecursorOutOfWater(true);
        }

        [ConsoleCommand("swimmode")]
        public static void SwimMode()
        {
            Player.main.SetPrecursorOutOfWater(false);
        }

        [ConsoleCommand("playeranimtrigger")]
        public static void PlayerAnimatorTrigger(string parameter)
        {
            Player.main.playerAnimator.SetTrigger(parameter);
        }

        [ConsoleCommand("playeranimbool")]
        public static void PlayerAnimatorBool(string parameter, bool value)
        {
            Player.main.playerAnimator.SetBool(parameter, value);
        }

        [ConsoleCommand("playeranimfloat")]
        public static void PlayerAnimatorFloat(string parameter, float value)
        {
            Player.main.playerAnimator.SetFloat(parameter, value);
        }

        [ConsoleCommand("copyposition")]
        public static void CopyPosition()
        {
            CopyPositionInternal(false);
        }

        [ConsoleCommand("copyrayposition")]
        public static void CopyRayPosition()
        {
            CopyPositionInternal(true);
        }

        private static void CopyPositionInternal(bool raycast)
        {
            Vector3 position;
            var cam = MainCamera.camera.transform;
            if (raycast && Targeting.GetTarget(Player.main.gameObject, 2000f, out GameObject target, out float distance))
            {
                position = cam.position + cam.forward * distance;
                ErrorMessage.AddMessage("Copying the position of a raycast from the crosshair to clipboard.");
            }
            else
            {
                ErrorMessage.AddMessage("Copying the position of the player camera to clipboard.");
                position = cam.position;
            }
            var formatted = Helpers.FormatVector3Constructor(position, 2);
            ErrorMessage.AddMessage("Formatted constructor: " + formatted);
            GUIUtility.systemCopyBuffer = formatted;
        }
    }
}
