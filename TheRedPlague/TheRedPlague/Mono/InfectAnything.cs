using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Story;
using UnityEngine.Serialization;

namespace TheRedPlague.Mono;

// add this script to any object to infect it lol
public class InfectAnything : MonoBehaviour, IStoryGoalListener
{
    public Renderer[] renderers;
    private List<Material> _materials;

    public bool infectedAtStart = true;
    public float infectionHeightStrength = 0.1f;

    public float infectionAmount = 4;
    
    private const string ShaderKeyWord = "UWE_INFECTION";
    
    private static readonly int InfectionHeightStrengthParameter = Shader.PropertyToID("_InfectionHeightStrength");

    public bool isAppliedToPlayer;

    private IEnumerator Start()
    {
        isAppliedToPlayer = gameObject.GetComponent<Player>() != null;
        
        yield return new WaitUntil(() =>
            StoryGoalManager.main.completedGoals != null && StoryGoalManager.main.completedGoals.Count > 0);
        if (StoryGoalManager.main.IsGoalComplete(StoryUtils.ForceFieldLaserDisabled.key))
        {
            ApplyShading(false);
            yield break;
        }
        StoryGoalManager.main.AddListener(this);
        ApplyShading(infectedAtStart);
    }

    public void ApplyShading(bool infected)
    {
        if (renderers == null || renderers.Length == 0)
        {
            if (isAppliedToPlayer)
            {
                renderers = transform.Find("body").GetComponentsInChildren<Renderer>(true).Where((r) => !r.gameObject.name.Contains("BoneArmor")).ToArray();
            }
            else
            {
                renderers = gameObject.GetComponentsInChildren<Renderer>(true);
            }
        }
        if (_materials == null)
        {
            _materials = new List<Material>();
            foreach (var renderer in renderers)
            {
                if (renderer != null)
                {
                    _materials.AddRange(renderer.materials);
                }
            }
        }
        foreach (var material in _materials)
        {
            if (material == null) continue;
            
            material.SetFloat(ShaderPropertyID._InfectionAmount, infectionAmount);
            material.SetVector(ShaderPropertyID._ModelScale, base.transform.localScale);
            if (infected)
            {
                material.EnableKeyword(ShaderKeyWord);
                material.SetTexture(ShaderPropertyID._InfectionAlbedomap, Plugin.ZombieInfectionTexture);
                material.SetFloat(InfectionHeightStrengthParameter, infectionHeightStrength);
            }
            else
            {
                material.DisableKeyword(ShaderKeyWord);
            }
        }
    }
    
    private void OnDestroy()
    {
        if (_materials == null) return;

        foreach (var material in _materials)
        {
            Destroy(material);
        }
        
        StoryGoalManager main = StoryGoalManager.main;
        if (main)
        {
            main.RemoveListener(this);
        }
    }

    public void NotifyGoalComplete(string key)
    {
        if (key == StoryUtils.ForceFieldLaserDisabled.key)
        {
            ApplyShading(false);
        }
    }
}