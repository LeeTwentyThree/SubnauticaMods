using DebugHelper.Managers;
using SMLHelper.V2.Commands;

namespace DebugHelper.Commands
{
    public static class ColliderCommands
    {

        /// <summary>
        /// Shows all colliders in wanted range
        /// </summary>
        [ConsoleCommand("showcolliders")]
        public static void ShowCollidersInRange(float range = 50f, bool hideMessage = false)
        {
            DebugCollidersManager.main.ShowCollidersRange(range);
            if (!hideMessage) ErrorMessage.AddMessage($"All colliders in {DebugCollidersManager.main.targetRange}m range are visible now.");
        }
        /// <summary>
        /// Shows all colliders in wanted range (alias)
        /// </summary>
        [ConsoleCommand("sw_colls")]
        public static void ShowCollidersInRange2(float range = 50f, bool hideMessage = false) => ShowCollidersInRange(range, hideMessage);

        /// <summary>
        /// Hides all shown colliders
        /// </summary>
        [ConsoleCommand("hidecolliders")]
        public static void HideColliders(bool hideMessage = false)
        {
            DebugCollidersManager.main.HideColliders();
            if (!hideMessage) ErrorMessage.AddMessage($"Colliders are no longer visible.");
        }
        /// <summary>
        /// Hides all shown colliders (alias)
        /// </summary>
        [ConsoleCommand("hd_colls")]
        public static void HideColliders2(bool hideMessage = false) => HideColliders(hideMessage);
    }
}
