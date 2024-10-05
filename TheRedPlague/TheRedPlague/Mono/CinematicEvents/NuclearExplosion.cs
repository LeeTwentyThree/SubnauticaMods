using System.Collections;
using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono.CinematicEvents;

public class NuclearExplosion : MonoBehaviour
{
    public static void PlayCinematic()
    {
        new GameObject("NuclearExplosion").AddComponent<NuclearExplosion>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(5);
        var explosion = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Nuclear explosion"));
        var pos = new Vector3(1650, 0, 1122);
        explosion.transform.position = pos;
        yield return new WaitForSeconds(1.3f);
        Utils.PlayFMODAsset(AudioUtils.GetFmodAsset("NuclearExplosion"), pos);
        MainCameraControl.main.ShakeCamera(4, 8, MainCameraControl.ShakeMode.Cos);
        yield return new WaitForSeconds(4);
        Utils.PlayFMODAsset(AudioUtils.GetFmodAsset("NuclearShockwave"), pos);
    }
}