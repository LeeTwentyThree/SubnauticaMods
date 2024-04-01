using UnityEngine;

namespace EpicImprovedTextures;

public class PeriodicallyUpdateRenderers : MonoBehaviour
{
    private float _timeNextUpdate;

    private const float UpdateInterval = 3;
    
    private void Update()
    {
        if (Time.time < _timeNextUpdate) return;
        
        _timeNextUpdate = Time.time + UpdateInterval;
        UpdateAllRenderers();
    }

    private void UpdateAllRenderers()
    {
        var database = TextureDatabase.GetInstance();
        var renderers = FindObjectsOfType<Renderer>();
        foreach (var renderer in renderers)
        {
            TextureUtils.ConvertRenderer(renderer, database);
        }
    }
}