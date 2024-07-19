using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono;

public class StalkingSoundsPlayer : MonoBehaviour
{
    private float _timeTryFootStepsAgain;
    private float _timeTryHatchAgain;

    private static readonly FMODAsset _footstepsSound = AudioUtils.GetFmodAsset("RandomFootsteps");
    private static readonly FMODAsset _hatchSound = AudioUtils.GetFmodAsset("event:/sub/base/enter_hatch");

    private void Start()
    {
        ResetFootstepTimer();
        ResetHatchTimer();
    }

    private void Update()
    {
        if (Time.time >= _timeTryFootStepsAgain)
        {
            ResetFootstepTimer();
            if (Player.main.GetCurrentSub() == null && Player.main.GetBiomeString() != "crashedShip") return;
            Utils.PlayFMODAsset(_footstepsSound, MainCamera.camera.transform.position + Random.onUnitSphere);
        }
        if (Time.time >= _timeTryHatchAgain)
        {
            ResetHatchTimer();
            if (Player.main.GetCurrentSub() == null) return;
            Utils.PlayFMODAsset(_hatchSound, MainCamera.camera.transform.position + Random.onUnitSphere * 20);
        }
    }

    private void ResetFootstepTimer()
    {
        _timeTryFootStepsAgain = Time.time + Random.Range(5 * 60, 10 * 60);
    }
    
    private void ResetHatchTimer()
    {
        _timeTryHatchAgain = Time.time + Random.Range(15 * 60, 30 * 60);
    }
}