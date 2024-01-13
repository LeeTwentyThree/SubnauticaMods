using UnityEngine;

namespace TheRedPlague;

public static class JumpScareUtils
{
    public static bool IsPositionOnScreen(Vector3 position, float threshold = -0.1f)
    {
        var camTransform = MainCamera.camera.transform;
        return Vector3.Dot(camTransform.forward, (position - camTransform.position).normalized) > threshold;
    }
    
    public static bool TryGetSpawnPosition(out Vector3 pos, float radius, int maxIterations, float minDistanceFromPlayer)
    {
        if (Player.main.IsInBase())
        {
            for (int i = 0; i < maxIterations; i++)
            {
                // not gonna fix this typo sorry
                var angel = Random.value * 2 * Mathf.PI;
                var raycastStartPos =
                    Player.main.transform.position + new Vector3(Mathf.Cos(angel), 0, Mathf.Sin(angel)) * Random.Range(5, 25) + Vector3.up;
                if (Physics.Raycast(raycastStartPos, Vector3.down, out var hit, 3, -1, QueryTriggerInteraction.Ignore))
                {
                    if (!IsPositionOnScreen(hit.point))
                    {
                        pos = hit.point;
                        return true;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < maxIterations; i++)
            {
                var raycastStartPos =
                    Player.main.transform.position + (Random.onUnitSphere * radius) + Vector3.up * radius;
                if (Physics.Raycast(raycastStartPos, Vector3.down, out var hit, 60, -1, QueryTriggerInteraction.Ignore))
                {
                    if (!IsPositionOnScreen(hit.point) && Vector3.SqrMagnitude(MainCamera.camera.transform.position - hit.point) > minDistanceFromPlayer * minDistanceFromPlayer)
                    {
                        pos = hit.point;
                        return true;
                    }
                }
            }
        }

        pos = default;
        return false;
    }
}