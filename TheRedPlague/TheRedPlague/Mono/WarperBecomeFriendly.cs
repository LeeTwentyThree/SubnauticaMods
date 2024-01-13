using Story;
using UnityEngine;

namespace TheRedPlague.Mono;

public class WarperBecomeFriendly : MonoBehaviour, IStoryGoalListener
{
    private void Start()
    {
        if (StoryGoalManager.main.IsGoalComplete(StoryUtils.PlagueHeartGoal.key))
        {
            BecomeFriendly();
        }
        else
        {
            StoryGoalManager.main.AddListener(this);
        }
    }

    public void BecomeFriendly()
    {
        var creature = GetComponent<Creature>();
        creature.SetFriend(Player.main.gameObject);
        creature.friendlyToPlayer = true;
        Destroy(this);
    }

    public void NotifyGoalComplete(string key)
    {
        if (key == StoryUtils.PlagueHeartGoal.key)
        {
            BecomeFriendly();
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