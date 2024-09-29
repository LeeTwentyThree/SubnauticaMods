using UnityEngine;

namespace TheRedPlague.Mono;

public class InfectedVehicle : MonoBehaviour
{
    private void Start()
    {
        GetComponent<LiveMixin>().health = 0;
    }
}