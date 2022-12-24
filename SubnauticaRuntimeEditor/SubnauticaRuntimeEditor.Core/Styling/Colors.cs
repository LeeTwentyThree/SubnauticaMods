using UnityEngine;

namespace SubnauticaRuntimeEditor.Core.Styling
{
    internal static class Colors
    {
        public static Color defaultColor = Color.white;
        public static Color defaultTranparentColor = new Color(1, 1, 1, 0.75f);
        public static Color resetWindowsButtonColor = new Color(1, 1, 1, 0.75f);
        public static Color windowManagerSelectedColor = genericSelectionColor;
        public static Color inspectorTabSelectedColor = genericSelectionColor;
        public static Color inspectorHierarchySelectedColor = genericSelectionColor;
        public static Color objectViewerSelectedColor = genericSelectionColor;

        public static Color gizmosColor = new Color(1, 0, 0.6f);
        public static Color colliderGizmosColor = new Color(0, 1, 0);
        public static Color meshColliderGizmosColor = Color.magenta;
        public static Color terrainColliderGizmosColor = Color.magenta;
        public static Color boneColliderInsideGizmoColor = Color.red;
        public static Color boneColliderOutsideGizmoColor = Color.yellow;

        public static Color genericSelectionColor => new Color(1, 1, 0); //new Color(1, 0.7f, 0f);
    }
}
