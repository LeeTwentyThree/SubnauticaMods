using UnityEngine;

namespace AggressiveFauna;

internal class ApocalypseMusic : MonoBehaviour
{
    private static ApocalypseMusic instance;

    private float musicTimePassed;

    private static FMODAsset music;
    private static float musicDuration;

    public static void Play()
    {
        if (CanPlay())
        {
            instance = new GameObject("ApocalypseMusic").AddComponent<ApocalypseMusic>();
        }
    }

    public static bool CanPlay()
    {
        return instance == null;
    }

    private bool MusicIsOver
    {
        get
        {
            return musicTimePassed > musicDuration;
        }
    }

    private bool MusicIsPaused
    {
        get
        {
            return UWE.FreezeTime.freezers.Count != 0;
        }
    }

    private void Start()
    {
        Utils.PlayFMODAsset(music, Player.main.transform.position);
    }

    private void Update()
    {
        if (MusicIsOver)
        {
            Destroy(gameObject);
        }
        if (!MusicIsPaused)
        {
            musicTimePassed += Time.unscaledDeltaTime;
        }
    }

    public static void SetupMusic(FMODAsset asset, float musicDuration)
    {
        music = asset;
        ApocalypseMusic.musicDuration = musicDuration;
    }
}
