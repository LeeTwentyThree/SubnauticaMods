using UnityEngine;
using UnityEngine.EventSystems;

namespace DebugHelper.Systems
{
    public class DebugIconButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public System.Action onInteract;
        public bool Hovered { get; private set; }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Hovered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Hovered = false;
        }

        private void Update()
        {
            if (Hovered)
            {
                var key1 = Main.config.InteractWithDebugIconKey1;
                var key2 = Main.config.InteractWithDebugIconKey2;
                if (Input.GetKeyDown(key1) && Input.GetKey(key2) || Input.GetKey(key1) && Input.GetKeyDown(key2))
                {
                    OnInteract();
                }
            }
        }

        private void OnInteract()
        {
            if (onInteract != null)
            {
                onInteract();
            }
        }
    }
}
