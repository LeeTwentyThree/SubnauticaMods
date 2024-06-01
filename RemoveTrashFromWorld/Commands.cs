using System;
using Nautilus.Commands;
using UnityEngine;

namespace RemoveTrashFromWorld;

public static class Commands
{
    [ConsoleCommand("cleanup")]
    public static void CleanUp(string itemId, float distanceFromPlayer)
    {
        var identifiers = UniqueIdentifier.identifiers;
        foreach (var identifier in identifiers)
        {
            if (identifier.Value == null || identifier.Value is not PrefabIdentifier prefabIdentifier)
            {
                continue;
            }

            if (Vector3.Distance(prefabIdentifier.transform.position, MainCamera.camera.transform.position) >
                distanceFromPlayer)
            {
                continue;
            }
            
            if (ObjectMatchesId(prefabIdentifier, itemId))
            {
                UnityEngine.Object.Destroy(prefabIdentifier.gameObject);
            }
        }

        var globalRoot = LargeWorldStreamer.main.globalRoot;
        foreach (var prefabIdentifier in globalRoot.GetComponentsInChildren<PrefabIdentifier>())
        {
            if (Vector3.Distance(prefabIdentifier.transform.position, MainCamera.camera.transform.position) >
                distanceFromPlayer)
            {
                continue;
            }
            
            if (ObjectMatchesId(prefabIdentifier, itemId))
            {
                UnityEngine.Object.Destroy(prefabIdentifier.gameObject);
            }
        }
    }

    private static bool ObjectMatchesId(PrefabIdentifier identifier, string id)
    {
        if (identifier.ClassId.Equals(id, StringComparison.OrdinalIgnoreCase))
            return true;
        if (CraftData.GetTechType(identifier.gameObject).ToString().Equals(id, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}