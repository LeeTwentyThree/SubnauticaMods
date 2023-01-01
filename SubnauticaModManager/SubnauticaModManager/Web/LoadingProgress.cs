namespace SubnauticaModManager.Web;

public class LoadingProgress
{
    public static LoadingProgress current;

    public string Status { get; set; }
    public float Progress { get; set; }

    public LoadingProgress()
    {
        SetActive();
    }

    public void Complete()
    {
        current = null;
    }

    public void SetActive()
    {
        if (current != null && current != this)
        {
            Plugin.Logger.LogError("TWO INSTANCES OF THE LOADINGPROGRESS CLASS EXIST AT ONCE!");
        }
        current = this;
    }

    public void SetStatusForError(string error)
    {
        Status = error;
    }
}
