using System;
using UnityEngine;

namespace TheRedPlague.Mono;

public class LookAtPlayer : MonoBehaviour
{
    private void Update()
    {
        var diff = transform.position - MainCamera.camera.transform.position;
        diff.y = 0;
        transform.forward = diff.normalized;
    }
}