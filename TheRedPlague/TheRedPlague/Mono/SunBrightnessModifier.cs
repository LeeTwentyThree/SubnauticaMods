using System;
using Story;
using UnityEngine;
using uSky;

namespace TheRedPlague.Mono;

public class SunBrightnessModifier : MonoBehaviour, IStoryGoalListener
{
    private float _defaultBrightness;
    private uSkyLight _skyLight;
    
    private void Start()
    {
        _skyLight = GetComponent<uSkyLight>();
        _defaultBrightness = _skyLight.SunIntensity;
        if (!StoryGoalManager.main.IsGoalComplete(StoryUtils.EnzymeRainEnabled.key))
        {
            _skyLight.SunIntensity *= 1f - Plugin.ModConfig.DarknessPercent * 0.01f;
            StoryGoalManager.main.AddListener(this);
        }
    }
    
    public void NotifyGoalComplete(string key)
    {
        if (key == StoryUtils.EnzymeRainEnabled.key)
        {
            _skyLight.SunIntensity *= _defaultBrightness;
        }
    }
    
    private void OnDestroy()
    {
        StoryGoalManager main = StoryGoalManager.main;
        if (main)
        {
            main.RemoveListener(this);
        }
    }
}