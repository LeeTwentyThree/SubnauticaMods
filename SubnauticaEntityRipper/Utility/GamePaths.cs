using System.IO;

namespace SubnauticaEntityRipper.Utility;

public class GamePaths
{
    private const string CellsCacheDirectory = @"Subnautica_Data\StreamingAssets\SNUnmanagedData\Build18\CellsCache";

    private const string BatchObjectsDirectory =
        @"Subnautica_Data\StreamingAssets\SNUnmanagedData\Build18\BatchObjectsCache";

    public static string GetCellsCacheDirectory() => Path.Combine(BepInEx.Paths.GameRootPath, CellsCacheDirectory);
    public static string GetBatchObjectsDirectory() => Path.Combine(BepInEx.Paths.GameRootPath, BatchObjectsDirectory);
}