namespace SubnauticaModManager.Web;

public class LoadingProgress
{
    public static LoadingProgress current;

    public string Status { get; set; }
    public float Progress { get; set; }

    public LoadingProgress()
    {
        if (current != null)
        {
            Plugin.Logger.LogError("TWO INSTANCES OF THE LOADINGPROGRESS CLASS EXIST AT ONCE!");
        }
        current = this;
    }

    public void Complete()
    {
        current = null;
    }
}
