using System.Collections;
using UnityEngine;

namespace BloopAndBlaza.Mono;

public class BloopVehicleCinematic : MonoBehaviour
{
    public GameObject throat;

    private bool _playing;
    private Vector3 _startPos;
    private Quaternion _startRot;
    private float _timeElapsed;

    private IEnumerator Start()
    {
        _playing = true;
        _startPos = transform.position;
        _startRot = transform.rotation;
        _timeElapsed = 0f;
        var killPlayer = Player.main.GetVehicle() == gameObject.GetComponent<Vehicle>();
        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;
        if (killPlayer)
        {
            MainCameraControl.main.ShakeCamera(4f, 1.5f, MainCameraControl.ShakeMode.BuildUp, 1.2f);
        }
        yield return new WaitForSeconds(1.5f);
        _playing = false;
        GetComponent<LiveMixin>().TakeDamage(20000f);
        if (killPlayer)
        {
            Player.main.liveMixin.Kill(DamageType.Normal);
        }
    }

    private void Update()
    {
        if (_playing)
        {
            transform.position = Vector3.Lerp(_startPos, throat.transform.position, _timeElapsed);
            transform.rotation = Quaternion.RotateTowards(_startRot, throat.transform.rotation, Time.deltaTime * 360f);
            _timeElapsed += Time.deltaTime / 2f;
        }
    }
}