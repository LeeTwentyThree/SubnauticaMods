using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ShipMod.Ship
{
    public class ShipSlidingDoor : MonoBehaviour
    {
        bool isOpen = false;
        float timeCanOpenAgain = 0f;
        Animator animator;
        AudioSource openSource;
        AudioSource closeSource;

        void Awake()
        {
            var buttons = GetComponentsInChildren<Button>();
            foreach(var button in buttons)
            {
                button.onClick.AddListener(OnToggle);
            }

            animator = GetComponent<Animator>();
            openSource = Helpers.FindChild(gameObject, "OpenSound").GetComponent<AudioSource>();
            closeSource = Helpers.FindChild(gameObject, "CloseSound").GetComponent<AudioSource>();
        }

        void OnToggle()
        {
            if(Time.time > timeCanOpenAgain)
            {
                isOpen = !isOpen;
                if (isOpen)
                {
                    openSource.Play();
                }
                else
                {
                    closeSource.Play();
                }
                timeCanOpenAgain = Time.time + 1f;
                animator.SetBool("open", isOpen);
            }
        }
    }
}
