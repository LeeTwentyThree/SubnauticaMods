using TheRedPlague.PrefabFiles;
using UnityEngine;

namespace TheRedPlague.Mono.CreatureBehaviour.Drifter;

public class DrifterHoverAboveTerrain : CreatureAction
{
    public float maxDistanceAboveTerrain = 80;
    public float desiredDistanceAboveTerrain = 40;
    public float minDistanceAboveTerrain = 30;
    public float maxDepthToConsider = 800;
    public float chanceToBeABottom = 0.5f;

    private const float UpdateRate = 3f;
    
    private float _targetHeight;
    private bool _active;
    
    private void Start()
    {
        if (Random.value <= chanceToBeABottom || transform.position.y < -maxDepthToConsider)
        {
            InvokeRepeating(nameof(CheckForTerrain), Random.value, UpdateRate);
        }
    }

    public override float Evaluate(Creature creature, float time)
    {
        if (!_active) return 0f;
        var yPos = transform.position.y;
        if (yPos > _targetHeight + maxDistanceAboveTerrain || yPos < _targetHeight + minDistanceAboveTerrain) return evaluatePriority;
        return 0f;
    }

    public override void Perform(Creature creature, float time, float deltaTime)
    {
        if (!_active) return;
        swimBehaviour.SwimTo(
            new Vector3(
                transform.position.x,
                _targetHeight + desiredDistanceAboveTerrain,
                transform.position.z),
            DrifterPrefab.BaseVelocity);
    }

    private void CheckForTerrain()
    {
        if (!isActiveAndEnabled)
        {
            _active = false;
            return;
        }
        
        if (!WorldHeightLib.HeightMap.Instance.TryGetValueAtPosition(new Vector2(transform.position.x, transform.position.z), out var val))
        {
            _active = false;
            return;
        }

        if (val < -maxDepthToConsider || transform.position.y < val - 10)
        {
            _active = false;
            return;
        }

        _targetHeight = val;
        _active = true;
    }
}