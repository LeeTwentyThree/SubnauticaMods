using UnityEngine;

namespace TheRedPlague.Mono.CreatureBehaviour.Drifter;

public class DrifterEyeTracking : MonoBehaviour
{
    public Transform[] transforms;

    private void LateUpdate()
    {
        var playerPos = MainCamera.camera.transform.position;
        foreach (var trans in transforms)
        {
            trans.up = (playerPos - trans.position).normalized;
        }
    }
}