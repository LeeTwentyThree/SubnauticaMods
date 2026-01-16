using UnityEngine;

namespace SeaVoyager.Mono;

public class ChangeWaterDepth : MonoBehaviour
{
    public WorldForces worldForces;
    public float newDepth;
    
    private void Start()
    {
        worldForces.waterDepth = newDepth;
    }
}