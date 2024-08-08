using Nautilus.Assets;

namespace TheRedPlague.PrefabFiles;

public static class DrifterCannonPrefab
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("DrifterCannon")
        .WithSizeInInventory(new Vector2int(2, 2));

    public static void Register()
    {
        
    }
}