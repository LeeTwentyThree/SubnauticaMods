using UnityEngine;

namespace TheRedPlague.Mono.PlagueGarg;

public class DisableAuroraRadiationOnStart : MonoBehaviour
{
    private void Start()
    {
        var leakingRadiation = LeakingRadiation.main;
        if (leakingRadiation)
        {
            leakingRadiation.GetComponent<RadiatePlayerInRange>().enabled = false;
        }
    }
}