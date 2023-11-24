using System;
using System.Linq;
using UnityEngine;

namespace WeatherMod.Mono;

public class RandomLightningModel : MonoBehaviour
{
    private static Mesh[] _lightningMeshes;
    
    private void Awake()
    {
        _lightningMeshes ??= Plugin.AssetBundle.LoadAllAssets<Mesh>().Where(m => m.name.Contains("Lightning")).ToArray();

        GetComponentInChildren<ParticleSystemRenderer>().mesh = _lightningMeshes.GetRandomUnity();
    }
}