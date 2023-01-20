namespace ResourceBonanza;

internal class Randomizer
{
    internal static OutcropRandomizer outcropRandomizer = new();

    internal static Randomizer resourceRandomizer = new();

    public TechType GetRandomTechType()
    {
        if (validTechTypes == null) UpdateTechTypeList();
        if (validTechTypes == null || validTechTypes.Count == 0) return TechType.Unobtanium;
        return validTechTypes[Random.Range(0, validTechTypes.Count)];
    }
}

internal class OutcropRandomizer : Randomizer
{
    public IEnumerator SpawnOutcropItems(BreakableResource outcrop, AssetReferenceGameObject breakPrefab)
    {
        var techType = GetRandomTechType();
        var amount = GetRandomAmount(IsTechTypeForCreature(techType));
        var task = CraftData.GetPrefabForTechTypeAsync(techType);
        yield return task;
        var prefab = task.GetResult();
        if (prefab == null) yield break;
        for (int i = 0; i < amount; i++)
        {
            var spawnPosition = outcrop.transform.position + outcrop.transform.up * outcrop.verticalSpawnOffset;
            if (amount > 1) spawnPosition += Random.insideUnitSphere * (amount / 18f);
            UWE.CoroutineHost.StartCoroutine(SpawnResourceFromPrefab(prefab, spawnPosition, outcrop.transform.up));
        }
    }

    private static IEnumerator SpawnResourceFromPrefab(GameObject prefab, Vector3 position, Vector3 up)
    {
        var spawned = Object.Instantiate(prefab, position, default);
        spawned.SetActive(true);
        Rigidbody rigidbody = spawned.EnsureComponent<Rigidbody>();
        UWE.Utils.SetIsKinematicAndUpdateInterpolation(rigidbody, false, false);
        rigidbody.AddTorque(Vector3.right * Random.Range(3, 6));
        rigidbody.AddForce(up * 0.1f);
        yield break;
    }

    public int GetRandomAmount(SpawnType spawnType)
    {
        if (spawnType == SpawnType.Crazy) return Random.Range(1, 4); // 1 to 3 for crazy spawns
        var value = Random.value;
        if (value < 0.05f) return 0;                    // 5% chance to return 0 items (trolled)
        if (value < 0.15f) return 1;                    // 10% chance to return 1 item
        if (value < 0.5f) return Random.Range(1, 51);   // 35% chance to return 1 to 50 items
        return Random.Range(1, 13);                     // 50% chance to return 1 to 12 items
    }

    private enum SpawnType
    {
        Item,
        Crazy
    }
}