using UWE;

namespace ModStructureHelperPlugin.Tools;

public static class PrefabUpDirectionManager
{
    public static UpDirection GetUpDirectionForPrefab(string classId)
    {
        if (WorldEntityDatabase.TryGetInfo(classId, out var info))
        {
            return info.prefabZUp ? UpDirection.Z : UpDirection.Y;
        }

        return UpDirection.Y;
    }
}

public enum UpDirection
{
    Y,
    Z
}