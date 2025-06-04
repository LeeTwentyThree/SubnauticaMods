using PdaUpgradeCards.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PdaUpgradeCards.MonoBehaviours.UI;

public class MusicPlayerUI : MonoBehaviour, IManagedUpdateBehaviour
{
    public static MusicPlayerUI Main { get; private set; }

    public TextMeshProUGUI currentMusicText;
    public Slider volumeSlider;
    public Slider progressBar;
    public Image playButtonImage;
    public Sprite playSprite;
    public Sprite pauseSprite;
    public Sprite restartSprite;

    private bool _playing;

    private PdaMusic _currentMusic;
    private AudioSource _emitter;

    private int _currentTrackNumber;

    public void OnMusicCardDestroyed()
    {
        StopOldTrackAndFadeOutIfNecessary();
    }

    private void Start()
    {
        Main = this;
        SetCurrentTrack(0);
    }

    private void OnEnable()
    {
        BehaviourUpdateUtils.Register(this);
    }

    private void OnDisable()
    {
        BehaviourUpdateUtils.Deregister(this);
    }

    private void OnDestroy()
    {
        if (_emitter != null)
            Destroy(_emitter.gameObject);
    }

    private void SetCurrentTrack(int songIndex)
    {
        bool wasPlaying = _playing;
        
        _currentTrackNumber = songIndex;

        var allMusic = PdaMusicDatabase.GetAllMusic();
        if (allMusic.Count == 0)
        {
            Plugin.Logger.LogWarning("No music found!");
            currentMusicText.text = "No music to play!";
            return;
        }

        _currentMusic = allMusic[songIndex].Music;
        currentMusicText.text = $"{songIndex + 1}. {_currentMusic.GetTrackName()}";
        
        UpdateProgressPercent(0);
        
        StopOldTrackAndFadeOutIfNecessary();
        
        _emitter = new GameObject("PdaMusicEmitter").AddComponent<AudioSource>();
        _emitter.volume = volumeSlider.value;
        _emitter.clip = _currentMusic.SoundAsset;
        
        SetMusicPlaying(wasPlaying);
    }

    private void SetMusicPlaying(bool newPlayingState)
    {
        _playing = newPlayingState;
        
        playButtonImage.sprite = newPlayingState ? pauseSprite : playSprite;

        if (_currentMusic == null)
        {
            Plugin.Logger.LogWarning("No music found!");
            return;
        }

        if (newPlayingState)
        {
            _emitter.Play();
        }
        else
        {
            _emitter.Pause();
        }
    }

    public void OnPlayButton()
    {
        SetMusicPlaying(!_playing);
    }

    public void OnNextButton()
    {
        SetCurrentTrack(ProperModulo(_currentTrackNumber + 1, PdaMusicDatabase.GetTrackCount()));
    }

    public void OnPreviousButton()
    {
        SetCurrentTrack(ProperModulo(_currentTrackNumber - 1, PdaMusicDatabase.GetTrackCount()));
    }

    public void OnProgressBarChanged(float newValue)
    {
        if (Mathf.Abs(newValue - _emitter.time / _currentMusic.GetDuration()) < Time.deltaTime * 1.5f)
            return;
        _emitter.time = newValue * _currentMusic.GetDuration();
    }
    
    public void OnVolumeSliderChanged(float newValue)
    {
        _emitter.volume = newValue;
    }

    private static int ProperModulo(int a, int b)
    {
        return a - b * Mathf.RoundToInt(Mathf.Floor((float)a / b));
    }

    public void OnOpenFolderButton()
    {
        Application.OpenURL(PdaMusicDatabase.CustomTracksDirectory);
    }

    private void UpdateProgressPercent(float percentNormalized)
    {
        progressBar.value = percentNormalized;
    }

    private void StopOldTrackAndFadeOutIfNecessary()
    {
        UpdateProgressPercent(0);
        _playing = false;
        if (_emitter == null) return;
        var fader = _emitter.gameObject.AddComponent<AudioSourceFader>();
        fader.source = _emitter;
        fader.fadeDuration = 0.5f;
        Destroy(_emitter.gameObject, 1);
        _emitter = null;
    }


    public string GetProfileTag()
    {
        return "MusicPlayerUI";
    }

    public void ManagedUpdate()
    {
        if (_playing)
            UpdateProgressPercent(Mathf.Clamp01(_emitter.time / _currentMusic.GetDuration()));
    }

    public int managedUpdateIndex { get; set; }
}