using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono;

public class RandomFootstepsPlayer : MonoBehaviour
{
    private float _timeTryAgain;

    private static readonly FMODAsset _footstepsSound = AudioUtils.GetFmodAsset("RandomFootsteps");

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        if (Time.time < _timeTryAgain) return;
        ResetTimer();
        if (Player.main.GetCurrentSub() == null && Player.main.GetBiomeString() != "crashedShip") return;
        Utils.PlayFMODAsset(_footstepsSound, MainCamera.camera.transform.position + Random.onUnitSphere);
    }

    private void ResetTimer()
    {
        _timeTryAgain = Time.time + Random.Range(5 * 60, 10 * 60);
    }
}