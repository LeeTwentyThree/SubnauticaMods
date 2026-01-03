using System.Collections.Generic;
using UnityEngine;

namespace KallieʼsPropPack.MonoBehaviours;

public class LabLightFlicker : MonoBehaviour, IManagedUpdateBehaviour
{
    private static readonly int GlowStrength = Shader.PropertyToID("_GlowStrength");
    private static readonly int GlowStrengthNight = Shader.PropertyToID("_GlowStrengthNight");
    public bool forceLightOff;
    public float minFlickerSpeed = 0.04f;
    public float maxFlickerSpeed = 0.2f;
    public float minLightIntensity = 0.1f;
    public float maxLightIntensity = 1f;
    public Renderer[] renderers;
    public Light[] lights;

    private const float MinFlickerDurationForNoFlashing = 0.5f;
    private const float MaxFlickerDurationForNoFlashing = 0.8f;

    private float _timeNextChange;
    
    private List<Material> _materials = new();
    
    public int managedUpdateIndex { get; set; }

    private float[] _defaultLightIntensities;

    private void Start()
    {
        foreach (var renderer in renderers)
        {
            _materials.AddRange(renderer.materials);
        }
        
        _defaultLightIntensities = new float[lights.Length];
        for (int i = 0; i < lights.Length; i++)
        {
            _defaultLightIntensities[i] = lights[i].intensity;
        }

        if (forceLightOff)
        {
            foreach (var material in _materials)
            {
                SetMaterialBrightness(material, 0);
            }
            SetLightBrightness(0);
        }
        else
        {
            BehaviourUpdateUtils.Register(this);
        }
    }
    
    public string GetProfileTag()
    {
        return "TRP:LabLightFlicker";
    }

    public void ManagedUpdate()
    {
        if (Time.time < _timeNextChange)
        {
            return;
        }
        
        var glowStrength = Random.Range(minLightIntensity, maxLightIntensity);
        SetLightBrightness(glowStrength);
        foreach (var material in _materials)
        {
            SetMaterialBrightness(material, glowStrength);
        }
        if (MiscSettings.flashes)
            _timeNextChange = Time.time + Random.Range(minFlickerSpeed, maxFlickerSpeed);
        else
            _timeNextChange = Time.time + Random.Range(MinFlickerDurationForNoFlashing, MaxFlickerDurationForNoFlashing);
    }

    private void SetLightBrightness(float intensity)
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].intensity = intensity * _defaultLightIntensities[i];
        }
    }

    private void OnEnable()
    {
        BehaviourUpdateUtils.Register(this);
    }

    private void OnDisable()
    {
        BehaviourUpdateUtils.Deregister(this);
    }

    private void OnDestroy()
    {
        if (_materials == null) return;
        foreach (var material in _materials)
        {
            Destroy(material);
        }
    }

    private static void SetMaterialBrightness(Material material, float brightness)
    {
        if (material)
        {
            material.SetFloat(GlowStrength, brightness);
            material.SetFloat(GlowStrengthNight, brightness);
        }
    }
}