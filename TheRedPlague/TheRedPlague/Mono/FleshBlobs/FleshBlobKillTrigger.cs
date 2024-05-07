using UnityEngine;

namespace TheRedPlague.Mono.FleshBlobs;

public class FleshBlobKillTrigger : MonoBehaviour
{
    public FleshBlobKillTriggerManager manager;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player.main.gameObject)
        {
            manager.KillPlayer(this.gameObject);
        }
    }
}