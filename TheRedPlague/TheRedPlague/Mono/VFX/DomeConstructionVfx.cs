using System.Collections.Generic;
using Nautilus.Utility;
using Story;
using UnityEngine;

namespace TheRedPlague.Mono.VFX;

public class DomeConstructionVfx : MonoBehaviour
{
    public Renderer domeShieldRenderer;
    public Texture emissiveTex;
    
    private Renderer[] _renderers;
    private List<Material> _materials;

    private float _constructionTimeStarted;
    private bool _constructing;
    
    private const float KConstructionDuration = 45;

    private void SetUpGhostMaterials()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        _materials = new List<Material>();
        
        foreach (var renderer in _renderers)
        {
            _materials.AddRange(renderer.materials);
        }

        foreach (var material in _materials)
        {
            if (material.shader == null || material.shader.name == "DontRender") continue;
            
            material.EnableKeyword("FX_BUILDING");
            material.SetTexture(ShaderPropertyID._EmissiveTex, emissiveTex);
            material.SetFloat(ShaderPropertyID._Cutoff, 0.4f);
            material.SetColor(ShaderPropertyID._BorderColor, new Color(0f, 1f, 2f, 1f));
            material.SetFloat(ShaderPropertyID._Built, 0f);
            material.SetFloat(ShaderPropertyID._Cutoff, 0.42f);
            material.SetVector(ShaderPropertyID._BuildParams, new Vector4(0.05f, 0.05f, 0.01f, -0.01f));
            material.SetFloat(ShaderPropertyID._NoiseStr, 0.25f);
            material.SetFloat(ShaderPropertyID._NoiseThickness, 0.65f);
            material.SetFloat(ShaderPropertyID._BuildLinear, 1f);
            material.SetFloat(ShaderPropertyID._MyCullVariable, 0f);
            material.SetFloat(ShaderPropertyID._minYpos, 0);
            material.SetFloat(ShaderPropertyID._maxYpos, 3000);
        }
    }

    private void Awake()
    {
        var goalManager = StoryGoalManager.main;
        if (goalManager && goalManager.IsGoalComplete(StoryUtils.DomeConstructionEvent.key))
        {
            return;
        }
        
        BeginConstruction();
    }

    private void BeginConstruction()
    {
        domeShieldRenderer.enabled = false;
        SetUpGhostMaterials();
        _constructionTimeStarted = Time.time;
        StoryGoalManager.main.OnGoalComplete(StoryUtils.DomeConstructionEvent.key);
        _constructing = true;
        Utils.PlayFMODAsset(AudioUtils.GetFmodAsset("DomeConstruction"));
    }

    private void Update()
    {
        if (!_constructing) return;
        if (Time.time > _constructionTimeStarted + KConstructionDuration)
        {
            OnConstructionEnded();
            _constructing = false;
            return;
        }

        var progress = (Time.time - _constructionTimeStarted) / KConstructionDuration;
        
        foreach (var material in _materials)
        {
            if (material.shader.name != "DontRender")
            {
                material.SetFloat(ShaderPropertyID._Built, progress);
            }
        }
    }

    private void OnConstructionEnded()
    {
        foreach (var material in _materials)
        {
            material.DisableKeyword("FX_BUILDING");
        }

        domeShieldRenderer.enabled = true;
    }

    private void OnDestroy()
    {
        if (_materials == null) return;
        foreach (var material in _materials)
        {
            Destroy(material);
        }
    }
}