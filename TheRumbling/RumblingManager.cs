using System.Collections;
using System.Collections.Generic;
using TheRumbling.Mono;
using TheRumbling.Prefabs;
using UnityEngine;

namespace TheRumbling;

internal static class RumblingManager
{
    private static List<WallTitan> _wallTitans = new List<WallTitan>();

    private static GameObject _foundingTitan;

    public static void RegisterTitan(WallTitan titan) => _wallTitans.Add(titan);

    public static void UnregisterTitan(WallTitan titan) => _wallTitans.Remove(titan);

    public static WallTitan GetNearestTitan(Vector3 toPosition)
    {
        var closestDistance = float.MaxValue;
        WallTitan nearest = null;
        foreach (var titan in _wallTitans)
        {
            var distance = (toPosition - titan.transform.position).sqrMagnitude;
            if (distance < closestDistance)
            {
                nearest = titan;
                closestDistance = distance;
            }
        }
        return nearest;
    }

    public static void BeginRumblingEvent()
    {
        RumblingSkyManager.ToggleRumblingSkybox(true);
        UWE.CoroutineHost.StartCoroutine(BeginRumblingAsync());
    }

    public static void KillAll()
    {
        foreach (var titan in _wallTitans)
        {
            titan.Ragdoll();
        }
    }
    
    public static void DeleteAll()
    {
        var titansToRemove = new List<WallTitan>(_wallTitans);
        foreach (var titan in titansToRemove)
        {
            if (titan)
                Object.Destroy(titan.gameObject);
        }
        Object.Destroy(_foundingTitan);
    }

    public static void EmoteAll(string emoteName)
    {
        foreach (var titan in _wallTitans)
        {
            if (string.IsNullOrEmpty(emoteName))
                titan.PlayRandomEmote();
            else
                titan.PlayEmoteByName(emoteName);
        }
    }

    private static IEnumerator BeginRumblingAsync()
    {
        // Destroy duplicate if needed - should only be one ever!
        if (_foundingTitan != null)
        {
            Object.Destroy(_foundingTitan);
        }
        
        Vector2 foundingTitanPosition2D = new Vector3(0, Balance.SpawnDistance + 270);
        var task = CraftData.GetPrefabForTechTypeAsync(FoundingTitanPrefab.Info.TechType);
        yield return task;
        _foundingTitan = Object.Instantiate(task.GetResult(), new Vector3(foundingTitanPosition2D.x, -200, foundingTitanPosition2D.y), Quaternion.LookRotation(Vector3.back));
        _foundingTitan.transform.localScale = Vector3.one * 2;
        _foundingTitan.SetActive(true);
        
        // a list of positions where X corresponds to the position in the row and Y corresponds to the position in the columns
        List<Vector2> positions = new List<Vector2>();

        for (var r = 0; r < Balance.FormationRows; r++)
        {
            var titansInThisRow = Balance.FormationUnitsPerRow + Balance.FormationAdditionUnitsPerEachRow * r;
            for (int i = 0; i < titansInThisRow; i++)
            {
                float xPosition = ((float)titansInThisRow / 2 - i) * Balance.HorizontalDistanceBetweenTitans + (r % 2 * (Balance.HorizontalDistanceBetweenTitans / 2));
                var wallTitanPosition2D = new Vector2(xPosition, Balance.SpawnDistance + Balance.DistanceBetweenRows * r);
                if (Vector2.Distance(wallTitanPosition2D, foundingTitanPosition2D) < 210)
                    continue;
                positions.Add(wallTitanPosition2D);
            }
        }

        UWE.CoroutineHost.StartCoroutine(SpawnTitansAtPositions(positions));
    }

    private static IEnumerator SpawnTitansAtPositions(List<Vector2> positions)
    {
        var task = CraftData.GetPrefabForTechTypeAsync(WallTitanPrefab.Info.TechType);
        yield return task;
        var titanPrefab = task.GetResult();

        foreach (var titanSpawnPoint in positions)
        {
            var titan = Object.Instantiate(titanPrefab, new Vector3(titanSpawnPoint.x, Balance.DefaultTitanSpawnY, titanSpawnPoint.y), Quaternion.LookRotation(Vector3.back));
            titan.SetActive(true);
            yield return null;
        }
    }
}