using UnityEngine;

namespace WeatherMod;

public abstract class WeatherEvent
{
    private GameObject _effectInstance;
    
    protected abstract GameObject EffectPrefab { get; }
    protected abstract float DestroyDelay { get; }
    protected abstract FogSettings Fog { get; } 
    public abstract float MinDuration { get; }
    public abstract float MaxDuration { get; }

    internal void BeginEvent()
    {
        if (EffectPrefab != null)
        {
            _effectInstance = Object.Instantiate(EffectPrefab);
        }
        
        OnEventBegin(_effectInstance);
        
        FogManager.ChangeCurrentFog(Fog);
    }

    internal void EndEvent()
    {
        OnEventEnd(_effectInstance);
        Object.Destroy(_effectInstance, DestroyDelay);
    }

    protected abstract void OnEventBegin(GameObject effectPrefab);
    
    protected abstract void OnEventEnd(GameObject effectPrefab);
}