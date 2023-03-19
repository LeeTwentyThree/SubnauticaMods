using UWE;

namespace AlterraFlux.Utility;

public static class CommonPrefabs
{
    public static IEnumerator GetMoonpoolWaterSurface(IOut<GameObject> gameObject)
    {
        var request = PrefabDatabase.GetPrefabForFilenameAsync("Assets/Prefabs/Base/GeneratorPieces/BaseMoonpool.prefab");
        yield return request;
        request.TryGetPrefab(out var pref);
        gameObject.Set(pref.transform.Find("Flood_BaseMoonPool/x_BaseWaterPlane").gameObject);
    }

    public static IEnumerator GetGhostMaterial(IOut<Material> material)
    {
        var request = CraftData.GetPrefabForTechTypeAsync(TechType.SolarPanel);
        yield return request;
        material.Set(request.GetResult().GetComponent<Constructable>().ghostMaterial);
    }
}
