using System.Collections.Generic;
using Nautilus.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace SeaVoyager.Mono
{
    public class ShipSlidingDoor : MonoBehaviour
    {
        private bool _isOpen;
        private float _timeCanOpenAgain;
        private Animator _animator;
        private FMOD_CustomEmitter _emitter;
        private readonly List<ShipUITooltip> _tooltips = new();
        
        private static readonly FMODAsset OpenSound = AudioUtils.GetFmodAsset("SvSlidingDoorOpen");
        private static readonly FMODAsset CloseSound = AudioUtils.GetFmodAsset("SvSlidingDoorClose");

        private void Awake()
        {
            var buttons = GetComponentsInChildren<Button>();
            foreach(var button in buttons)
            {
                button.onClick.AddListener(OnToggle);
                var tooltip = button.gameObject.AddComponent<ShipUITooltip>();
                tooltip.Init("Open door", true);
                _tooltips.Add(tooltip);
            }

            _animator = GetComponent<Animator>();
            _emitter = gameObject.AddComponent<FMOD_CustomEmitter>();
            _emitter.playOnAwake = false;
            _emitter.followParent = true;
            _emitter.restartOnPlay = true;

            RefreshTooltips();
        }

        private void OnToggle()
        {
            if (Time.time < _timeCanOpenAgain) return;
            
            _isOpen = !_isOpen;
            _emitter.SetAsset(_isOpen ? OpenSound : CloseSound);
            _emitter.Play();
            _timeCanOpenAgain = Time.time + 1f;
            _animator.SetBool("open", _isOpen);
            
            foreach (var tooltip in _tooltips)
            {
                tooltip.showTooltip = false;
                {
                    tooltip.showTooltip = true;
                }
            }
            Invoke(nameof(RefreshTooltips), 1f);
        }

        private void RefreshTooltips()
        {
            foreach (var tooltip in _tooltips)
            {
                tooltip.showTooltip = true;
                tooltip.displayText = Language.main.Get(_isOpen ? "SvSlidingDoorClose" : "SvSlidingDoorOpen");
            }
        }
    }
}
