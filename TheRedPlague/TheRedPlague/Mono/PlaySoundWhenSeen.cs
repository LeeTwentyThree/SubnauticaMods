using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono;

public class PlaySoundWhenSeen : MonoBehaviour
{
    public FMODAsset sound = AudioUtils.GetFmodAsset("CloseJumpScare");
    public float maxDistance = 8f;

    private void Update()
    {
        if (JumpScareUtils.IsPositionOnScreen(transform.position))
        {
            if (Vector3.Distance(MainCamera.camera.transform.position, transform.position) < maxDistance && Vector3.Dot(transform.forward, (MainCamera.camera.transform.position - transform.position).normalized) > 0.1f)
                Utils.PlayFMODAsset(sound, transform.position);
            Destroy(this);
        }
    }
}