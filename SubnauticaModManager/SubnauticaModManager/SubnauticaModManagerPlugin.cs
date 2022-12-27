namespace SubnauticaModManager;

[BepInPlugin(GUID, "SubnauticaModManager", Version)]
public class SubnauticaModManagerPlugin : BaseUnityPlugin
{
    public const string Version = "1.0.0";
    public const string GUID = "SubnauticaModManager";

    private static readonly Harmony harmony = new Harmony(GUID);

    private void Awake()
    {
        harmony.PatchAll();
    }
}
