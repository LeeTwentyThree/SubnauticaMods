using System;
using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono.CinematicEvents;

public class GargTeaserEvent : MonoBehaviour
{
    private GameObject _fleshBlobPrefab;
    
    public static void PlayCinematic()
    {
        new GameObject("GargTeaserEvent").AddComponent<GargTeaserEvent>();
    }
    
    private void Start()
    {
        var fleshBlobPrefab = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("FleshBlobPrefab"));
        fleshBlobPrefab.SetActive(false);
        MaterialUtils.ApplySNShaders(fleshBlobPrefab);
        fleshBlobPrefab.AddComponent<SkyApplier>().renderers = fleshBlobPrefab.GetComponentsInChildren<Renderer>();
        
    }

    private void OnDestroy()
    {
        Destroy();
    }
}