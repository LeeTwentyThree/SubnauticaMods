using System.Collections.Generic;
using UnityEngine;

namespace TheRedPlague.Mono.PlagueGarg;

public class CollapsibleCubeBehaviour : MonoBehaviour
{
    private static List<CollapsibleCubeBehaviour> _cubes = new();

    private PrefabIdentifier _identifier;

    private bool _falling;

    private void Awake()
    {
        _identifier = GetComponent<PrefabIdentifier>();
    }

    private void OnEnable()
    {
        _cubes.Add(this);
    }

    private void OnDisable()
    {
        _cubes.Remove(this);
    }

    public static void OnCableBroken(string cableId)
    {
        foreach (var cube in _cubes)
        {
            if (cube._falling) continue;
            if (cableId == "96f26b01-af0c-40f9-8f96-8ce357aee121" &&
                cube._identifier.Id == "8090ca00-4f86-4b8d-84f7-f68b3c7531fb" ||
                cableId == "4eeb5261-8c04-4e86-b9cf-ba450701b96c" &&
                cube._identifier.Id == "3fd75804-c9db-41fc-80db-e5f47025bac9")
            {
                cube.StartFalling();
            }
        }
    }

    private void StartFalling()
    {
        _falling = true;
        gameObject.AddComponent<FallIntoVoid>();
    }
}