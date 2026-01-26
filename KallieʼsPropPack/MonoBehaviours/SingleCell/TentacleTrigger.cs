using UnityEngine;

namespace KallieʼsPropPack.MonoBehaviours.SingleCell;

public class TentacleTrigger : MonoBehaviour
{
    public SclTentacleBehaviour tentacle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<SubRoot>() != null ||
            other.gameObject.GetComponentInParent<Vehicle>() != null)
        {
            tentacle.OnSensorFelt();
        }
    }
}