using UnityEngine.Networking;
using System.Collections;

namespace SubnauticaModManager.Web;

public static class SubmodicaAPI
{
    private const string key = "0XAWs3EuMW5PyvpLbFVf7DviL86QQPtP";

    private const string urlFormat = "https://submodica.xyz/api/search/{0}/{1}/{2}"; // 0 - game, 1 - token, 2 - url-encoded query

    private const int maxQueryLength = 128;

    private const float search_fakeLoadDuration = 1f;

    public const float errorDisplayDuration = 3f;

    public static class Game
    {
        public static string Subnautica = "sn1";
        public static string BelowZero = "sbz";

        public static string Current
        {
            get
            {
                return Subnautica;
            }
        }
    }

    public static bool IsValidSearchQuery(string query)
    {
        if (string.IsNullOrEmpty(query)) return false;
        if (query.Length > maxQueryLength) return false;
        return true;
    }

    private static string GetSearchURL(string query)
    {
        return string.Format(urlFormat, Game.Current, key, UnityWebRequest.EscapeURL(query.ToLower()));
    }

    public static IEnumerator SearchRecentlyUpdated(LoadingProgress loadingProgress)
    {
        yield return Search("recently_updated", loadingProgress);
    }

    public static IEnumerator SearchMostDownloaded(LoadingProgress loadingProgress)
    {
        yield return Search(string.Empty, loadingProgress);
    }

    public static IEnumerator Search(string query, LoadingProgress loadingProgress)
    {
        var url = GetSearchURL(query);
        using (var request = UnityWebRequest.Get(url))
        {
            loadingProgress.Status = "Fetching search results...";
            while (!request.isDone)
            {
                yield return null;
                loadingProgress.Progress = request.downloadProgress;
            }
            if (request.error != null)
            {
                loadingProgress.SetStatusForError(request.error);
                yield return new WaitForSeconds(errorDisplayDuration);
                loadingProgress.Complete();
                yield break;
            }
            yield return new WaitForSeconds(search_fakeLoadDuration);
        }

        loadingProgress.Complete();
    }

    private static IEnumerator DisplayMod(SubmodicaMod mod, LoadingProgress progress)
    {

    }
}