using System;
using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono;

public class DeathScare : MonoBehaviour
{
    private static FMODAsset _sound = AudioUtils.GetFmodAsset("CloseJumpScare");
    
    public static void PlayDeathScare()
    {
        Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("DeathScare")).AddComponent<DeathScare>();
    }

    private void Start()
    {
        Utils.PlayFMODAsset(_sound, Player.main.transform.position);
        MainCameraControl.main.ShakeCamera(10, 6, MainCameraControl.ShakeMode.Quadratic, 1.5f);
        Invoke(nameof(Kill), 1.5f);
        FadingOverlay.PlayFX(new Color(0.5f, 0f, 0f), 0.1f, 0.2f, 0.1f, 0.1f);
    }

    private void LateUpdate()
    {
        var camera = MainCamera.camera.transform;
        transform.position = camera.position + camera.forward * 0.5f;
        transform.LookAt(camera);
    }

    private void Kill()
    {
        Player.main.liveMixin.TakeDamage(1000);
        FadingOverlay.PlayFX(new Color(0.1f, 0f, 0f), 0.1f, 0.2f, 0.1f);
        Destroy(gameObject, 0.2f);
    }
}