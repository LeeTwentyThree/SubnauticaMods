namespace SubnauticaModManager;

[BepInPlugin(GUID, "SubnauticaModManager", Version)]
public class SubnauticaModManagerPlugin : BaseUnityPlugin
{
    public const string Version = "1.0.0";
    public const string GUID = "SubnauticaModManager";

    private static readonly Harmony harmony = new Harmony(GUID);

    new internal static ManualLogSource Logger { get; private set; }

    private void Awake()
    {
        Logger = base.Logger;
        harmony.PatchAll();
    }
}