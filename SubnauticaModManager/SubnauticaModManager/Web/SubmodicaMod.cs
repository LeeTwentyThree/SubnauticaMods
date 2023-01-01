using System.Collections;
using UnityEngine.Networking;

namespace SubnauticaModManager.Web;

[System.Serializable]
internal class SubmodicaMod
{
    private string url;
    private string profile_image;
    private string title;
    private string creator;
    private string tagline;
    private int views;
    private int downloads;
    private int likes;
    private string latest_version;
    private string subnautica_compatibility;
    private string created_at;
    private string updated_at;

    public string Url => url;
    public Sprite ProfileImage { get; private set; }
    public string Title => title;
    public string Creator => creator;
    public string Tagline => tagline;
    public string GetViewsString()
    {
        return FormatInteger(views);
    }
    public string GetDownloadsString()
    {
        return FormatInteger(downloads);
    }
    public string GetLikesString()
    {
        return FormatInteger(likes);
    }
    public string LatestVersion => latest_version;
    public string SubnauticaCompatibility => subnautica_compatibility;
    public string DateCreated => created_at;
    public string DateUpdated => updated_at;

    private string FormatInteger(int value)
    {
        if (value > 1000)
        {
            float thousandsPlace = value / 1000f;
            return thousandsPlace.ToString("F1");
        }
        return value.ToString();
    }

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(url);
    }

    public IEnumerator LoadData()
    {
        if (!string.IsNullOrEmpty(profile_image))
        {
            using (var request = UnityWebRequestTexture.GetTexture(profile_image, false))
            {
                
            }
        }
    }
}
