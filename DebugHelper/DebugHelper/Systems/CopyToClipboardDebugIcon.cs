using UnityEngine;

namespace DebugHelper.Systems
{
    public abstract class CopyToClipboardDebugIcon : BasicDebugIcon
    {
        public DebugIconButton button;

        public override Sprite Icon => DebugIconManager.Icons.Clipboard;

        public override void OnCreation(DebugIconInstance instance)
        {
            button = instance.gameObject.EnsureComponent<DebugIconButton>();
            button.onInteract = OnInteract;
        }

        public abstract void OnInteract();

        public override float Scale
        {
            get
            {
                if (button == null || !button.Hovered)
                {
                    return 1f;
                }
                return 1.2f;
            }
        }

        public static void CopyToClipboard(string text)
        {
            GUIUtility.systemCopyBuffer = text;
        }
    }
}
