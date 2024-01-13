using Story;
using UnityEngine;

namespace TheRedPlague.Mono;

public class LaserMaterialManager : MonoBehaviour, IStoryGoalListener
{
    private Renderer _renderer;
    
    private void Start()
    {
        _renderer = GetComponentInChildren<Renderer>(true);

        if (StoryGoalManager.main.IsGoalComplete(StoryUtils.DisableDome.key))
        {
            _renderer.enabled = false;
        }
        else if (StoryGoalManager.main.IsGoalComplete(StoryUtils.EnzymeRainEnabled.key))
        {
            SetColorGreen();
        }
        else if (StoryGoalManager.main.IsGoalComplete(StoryUtils.ForceFieldLaserDisabled.key))
        {
            SetColorYellow();
        }
        
        StoryGoalManager.main.AddListener(this);
    }

    public void NotifyGoalComplete(string key)
    {
        if (key == StoryUtils.DisableDome.key)
        {
            _renderer.enabled = false;
        }
        else if (key == StoryUtils.EnzymeRainEnabled.key)
        {
            Invoke(nameof(SetColorGreen), 13);
        }
        else if (key == StoryUtils.ForceFieldLaserDisabled.key)
        {
            Invoke(nameof(SetColorYellow), 13);
        }
    }

    private void SetColorYellow()
    {
        _renderer.material.color = new Color(3, 3, 0.977941f);
    }
    
    private void SetColorGreen()
    {
        _renderer.material.color = new Color(2, 4.5f, 1);
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