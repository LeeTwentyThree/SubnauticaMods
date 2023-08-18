using UnityEngine;

namespace CustomWaterLevel;

internal class FreezeLifepodWhenFar : MonoBehaviour
{
    private Rigidbody rb;
    private float threshold = 40;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        var playerPos2d = new Vector2(MainCamera.camera.transform.position.x, MainCamera.camera.transform.position.z);
        var myPos2d = new Vector2(transform.position.x, transform.position.z);
        rb.isKinematic = Vector2.Distance(playerPos2d, myPos2d) > threshold && transform.position.y < Ocean.GetOceanLevel();
    }
}
