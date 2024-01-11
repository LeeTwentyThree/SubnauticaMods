using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono;

// har har har har har har har har  har har har har har har har
public class JumpScares : MonoBehaviour
{
    private float _minDelay = 20;
    private float _maxDelay = 40;
    private float _radius = 40;
    private float _timeSpawnAgain;

    private static FMODAsset jumpscareSound = AudioUtils.GetFmodAsset("WarperJumpscare");
    
    private GameObject Spawn()
    {
        var model = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("SharkJumpscare"));
        MaterialUtils.ApplySNShaders(model);
        var despawn = model.AddComponent<DespawnWhenOffScreen>();
        despawn.despawnIfTooClose = true;
        despawn.minDistance = 25;
        despawn.waitUntilSeen = true;
        despawn.moveInstead = true;
        despawn.moveRadius = 35f;
        despawn.disappearWhenLookedAtForTooLong = true;
        return model;
    }

    private void Update()
    {
        if (Time.time > _timeSpawnAgain)
        {
            _timeSpawnAgain = Time.time + Random.Range(_minDelay, _maxDelay);
            JumpScare();
        }
    }

    private void JumpScare()
    {
        Utils.PlayFMODAsset(jumpscareSound, Player.main.transform.position);
        if (JumpScareUtils.TryGetSpawnPosition(out var pos, _radius, 50, _radius / 2f))
        {
            var model = Spawn();
            model.transform.position = pos;
            var diff = pos - Player.main.transform.position;
            diff.y = 0;
            model.transform.forward = diff.normalized;
        }
    }
}