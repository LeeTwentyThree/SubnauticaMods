using UnityEngine;
using UnityEngine.EventSystems;

namespace SeaVoyager.Mono
{
    public class ShipUITooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string displayText;
        public bool clickable;
        public bool showTooltip = true;
        private bool hovering;
        
        public void Init(string displayText, bool clickable = true)
        {
            this.displayText = displayText;
            this.clickable = clickable;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hovering = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hovering = false;
        }

        private void LateUpdate()
        {
            if (!showTooltip)
            {
                return;
            }
            if (hovering)
            {
                if (clickable)
                {
                    HandReticle.main.SetInteractText(displayText, false, HandReticle.Hand.Left);
                }
                else
                {
                    HandReticle.main.SetInteractText(displayText, false, HandReticle.Hand.None);
                }
            }
        }
    }
}
