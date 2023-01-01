using SubnauticaModManager.Web;

namespace SubnauticaModManager.Mono;

internal class LoadingPrompt : MonoBehaviour
{
    private Transform uiParent;
    private TextMeshProUGUI text;
    private RectTransform barFill;

    private void Awake()
    {
        uiParent = transform.Find("UI");
        text = uiParent.Find("StatusText").GetComponent<TextMeshProUGUI>();
        barFill = gameObject.SearchChild("Fill").GetComponent<RectTransform>();
    }

    private void Update()
    {
        var loadingProgress = LoadingProgress.current;
        if (loadingProgress == null)
        {
            uiParent.gameObject.SetActive(false);
            return;
        }
        uiParent.gameObject.SetActive(true);
        text.text = loadingProgress.Status;
        barFill.localScale = new Vector3(loadingProgress.Progress, 1, 1);
    }
}