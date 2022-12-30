namespace SubnauticaModManager.Mono;

internal class Tab : MonoBehaviour
{
    public Type type;

    internal enum Type
    {
        Install,
        Manage,
        Download
    }
}