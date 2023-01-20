namespace ResourceBonanza;

internal static class TechTypeRandomizer
{
    private static List<TechType> techTypes;
    private static List<TechType> pickupableTechTypes;
    private static bool _initialized;

    public static IEnumerator InitializeAll()
    {
        techTypes = new List<TechType>();
        pickupableTechTypes = new List<TechType>();

        // all tech types
        var techMapping = CraftData.techMapping;
        if (techMapping != null)
        {
            foreach (var entry in techMapping)
            {
                techTypes.Add(entry.Key);
            }
        }

        // pickupable tech types
        foreach (var techType in techTypes)
        {
            var task = CraftData.GetPrefabForTechTypeAsync(techType);
            yield return task;
            var prefab = task.GetResult();
            if (prefab == null) continue;
            if (prefab.GetComponent<Pickupable>() != null) pickupableTechTypes.Add()
        }

        _initialized = true;
    }
}
