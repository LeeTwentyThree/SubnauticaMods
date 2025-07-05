using UnityEngine;

namespace GenerateBasePrefabCommand;

public static class Utility
{
    public static string GetPathToChild(Transform child, Transform parent)
    {
        var subsequentIteration = false;
        var path = string.Empty;
        var evaluated = child;
        while (evaluated != parent)
        {
            if (subsequentIteration)
            {
                path = evaluated.name + "/" + path;
            }
            else
            {
                path = evaluated.name;
                subsequentIteration = true;
            }
            evaluated = evaluated.parent;
        }

        return path;
    }
}