using UnityEngine;

namespace SeaVoyager;

public static class Helpers
{
    public static GameObject FindChild(GameObject parent, string byName)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.gameObject.name == byName)
            {
                return child.gameObject;
            }

            var recursive = FindChild(child.gameObject, byName);
            if (recursive)
            {
                return recursive;
            }
        }

        return null;
    }

    public static FMODAsset GetFmodAsset(string path)
    {
        var asset = ScriptableObject.CreateInstance<FMODAsset>();
        asset.path = path;
        return asset;
    }
}

public static class GameObjectExtensions
{
    public static GameObject SearchChild(this GameObject gameObject, string byName)
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.name == byName)
            {
                return child.gameObject;
            }

            GameObject recursive = child.gameObject.SearchChild(byName);
            if (recursive)
            {
                return recursive;
            }
        }

        return null;
    }

    public static T SearchComponent<T>(this GameObject gameObject, string gameObjectName)
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.name == gameObjectName)
            {
                return child.gameObject.GetComponent<T>();
            }

            var recursive = child.gameObject.SearchChild(gameObjectName);
            if (recursive)
            {
                return recursive.GetComponent<T>();
            }
        }

        return default;
    }
}