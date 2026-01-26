using System;
using UnityEngine;

namespace KallieʼsPropPack.MonoBehaviours;

public class DestroyIfIdMatches : MonoBehaviour
{
    public string[] ids = Array.Empty<string>();
    
    private void Start()
    {
        if (ShouldIKillMyself())
        {
            KillSelf();
        }
    }

    private void KillSelf()
    {
        Destroy(gameObject);
    }

    private bool ShouldIKillMyself()
    {
        var identifier = gameObject.GetComponent<UniqueIdentifier>();
        if (identifier == null) return false;
        
        var myId = identifier.Id;
        
        foreach (var candidate in ids)
        {
            if (candidate == myId) return true;
        }

        return false;
    }
}