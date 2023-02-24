using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SeaVoyager.Ship
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
                    HandReticle.main.SetText(HandReticle.TextType.Hand, displayText, false, GameInput.Button.LeftHand);
                }
                else
                {
                    HandReticle.main.SetText(HandReticle.TextType.Hand, displayText, false, GameInput.Button.None);
                }
            }
        }
    }
}
