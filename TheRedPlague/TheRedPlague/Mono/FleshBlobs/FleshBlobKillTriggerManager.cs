using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono.FleshBlobs;

public class FleshBlobKillTriggerManager : MonoBehaviour
{
    public GameObject triggerParent;
    
    private bool _busy;
    private FleshBlobMovement _movement;
    
    private void Start()
    {
        InvokeRepeating(nameof(LazyUpdate), Random.value * 2f, 2);
        _movement = GetComponent<FleshBlobMovement>();
    }

    private void LazyUpdate()
    {
        triggerParent.SetActive(Vector3.SqrMagnitude(Player.main.transform.position - transform.position) < 150 * 150);
    }

    public void KillPlayer(GameObject trigger)
    {
        if (_busy) return;
        _busy = true;
        var arms = SpawnArms();
        arms.transform.position = Player.main.transform.position - (Player.main.transform.position - trigger.transform.position).normalized * 2f;
        arms.transform.LookAt(Player.main.transform.position);
        arms.transform.localScale = Vector3.one * 15;
        Destroy(arms, 6);
        Player.main.rigidBody.isKinematic = true;
        _movement.FreezeForSeconds(6);
        Invoke(nameof(Execute), 4);
        Invoke(nameof(Refresh), 15);
    }

    private GameObject SpawnArms()
    {
        var obj = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("FleshBlobGrabPrefab"));
        obj.AddComponent<SkyApplier>().renderers = GetComponentsInChildren<Renderer>(true);
        MaterialUtils.ApplySNShaders(obj);
        return obj;
    }

    private void Execute()
    {
        Player.main.rigidBody.isKinematic = false;
        Player.main.liveMixin.TakeDamage(4589413);
        if (NoDamageConsoleCommand.main != null && NoDamageConsoleCommand.main.GetNoDamageCheat())
        {
            Player.main.liveMixin.Kill();
        }
        // DeathScare.PlayMutantDeathScare("MutatedDiver4", true);
    }
    
    private void Refresh()
    {
        _busy = false;
    }
}