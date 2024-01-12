using Story;
using UnityEngine;

namespace TheRedPlague.Mono;

public class LaserMaterialManager : MonoBehaviour, IStoryGoalListener
{
    private Renderer _renderer;
    
    private void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
        
        if (StoryGoalManager.main.IsGoalComplete(StoryUtils.ForceFieldLaserDisabled.key))
        {
            SetColorYellow();
        }
        
        StoryGoalManager.main.AddListener(this);
    }

    public void NotifyGoalComplete(string key)
    {
        if (key == StoryUtils.ForceFieldLaserDisabled.key)
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
}