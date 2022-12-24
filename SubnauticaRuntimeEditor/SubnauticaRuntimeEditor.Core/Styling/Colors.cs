using UnityEngine;

namespace SubnauticaRuntimeEditor.Core.Styling
{
    internal static class Colors
    {
        public static Color defaultColor = Color.white;
        public static Color defaultTranparentColor = new Color(1, 1, 1, 0.75f);
        public static Color resetWindowsButtonColor = new Color(1, 1, 1, 0.75f);
        public static Color windowManagerSelectedColor = genericSelectionColorButton;
        public static Color inspectorTabSelectedColor = genericSelectionColorButton;
        public static Color inspectorHierarchySelectedColor = genericSelectionColorButton;
        public static Color objectViewerSelectedColor = genericSelectionColorText;

        public static Color gizmosColor = new Color(1, 0, 0.6f);
        public static Color colliderGizmosColor = new Color(0, 1, 0);
        public static Color meshColliderGizmosColor = Color.magenta;
        public static Color terrainColliderGizmosColor = Color.magenta;
        public static Color boneColliderInsideGizmoColor = Color.red;
        public static Color boneColliderOutsideGizmoColor = Color.yellow;

        public static Color genericSelectionColorButton => new Color(1, 1, 0); //new Color(1, 0.7f, 0f);
        public static Color genericSelectionColorText => new Color(1, 0.83f, 0); //new Color(1, 0.7f, 0f);
    }
}
