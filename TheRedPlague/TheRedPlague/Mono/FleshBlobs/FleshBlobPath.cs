using UnityEngine;

namespace TheRedPlague.Mono.FleshBlobs;

public class FleshBlobPath
{
    public Vector2[] Points { get; }

    public FleshBlobPath(params Vector2[] points)
    {
        Points = points;
    }

    public int GetClosestPointIndex(Vector2 position)
    {
        int closest = 0;
        var closestDist = float.MaxValue;
        for (int i = 0; i < Points.Length; i++)
        {
            var dist = Vector2.Distance(position, Points[i]);
            if (dist < closestDist)
            {
                closest = i;
                closestDist = dist;
            }
        }

        return closest;
    }
    
    
    public int GetNextPointIndex(int currentPoint)
    {
        return currentPoint + 1 >= Points.Length ? 0 : currentPoint + 1;
    }
}