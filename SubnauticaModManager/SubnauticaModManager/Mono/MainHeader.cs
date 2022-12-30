namespace SubnauticaModManager.Mono;

internal class MainHeader : MonoBehaviour
{
    private void Start()
    {
        var text = GetComponent<TextMeshProUGUI>();
        text.text = $"{Plugin.Name} - v{Plugin.Version}";
    }
}
