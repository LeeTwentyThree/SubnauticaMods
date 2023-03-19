namespace AlterraFlux;

internal interface IModPrefab
{
    PrefabInfo Info { get; }
    TechType Register();
    IEnumerator GetGameObject(IOut<GameObject> gameObject);
}
