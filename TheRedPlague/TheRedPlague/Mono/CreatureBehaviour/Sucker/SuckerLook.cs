using UnityEngine;

namespace TheRedPlague.Mono.CreatureBehaviour.Sucker;

public class SuckerLook : MonoBehaviour
{
    private void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            Quaternion.LookRotation(MainCamera.camera.transform.position - transform.position), Time.deltaTime * 180);
    }
}