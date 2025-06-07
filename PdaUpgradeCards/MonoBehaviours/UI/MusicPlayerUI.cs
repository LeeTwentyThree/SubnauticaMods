using System;
using PdaUpgradeCards.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PdaUpgradeCards.MonoBehaviours.UI;

public class MusicPlayerUI : MonoBehaviour, IManagedUpdateBehaviour
{
    private const string VolumeSaveKey = "PdaUpgradeCardsVolumePercent";
    private static readonly Color EnabledColor = new(0.2f, 1f, 0.4f, 1f);

    public static MusicPlayerUI Main { get; private set; }

    public TextMeshProUGUI currentMusicText;
    
    public Slider volumeSlider;
    public Slider progressBar;
    public Image playButtonImage;
    public Image volumeIndicatorImage;
    public Image loopButtonImage;
    public Image shuffleButtonImage;
    
    public Sprite playSprite;
    public Sprite pauseSprite;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    private bool _playing;

    private PdaMusic _currentMusic;
    private AudioSource _emitter;

    private int _currentTrackNumber;

    private bool _looping;
    private bool _shuffled;
    
    private MusicPlaylist _currentPlaylist;
    
    public void OnMusicCardDestroyed()
    {
        StopOldTrackAndFadeOutIfNecessary();
        playButtonImage.sprite = playSprite;
    }

    private void Start()
    {
        Main = this;
        if (PlayerPrefs.HasKey(VolumeSaveKey))
        {
            volumeSlider.value = PlayerPrefs.GetFloat(VolumeSaveKey);
        }
        BehaviourUpdateUtils.Register(this);
        PdaMusicDatabase.OnMusicDatabaseChanged += RefreshMusicDatabase;
        RefreshMusicDatabase();
        
        SetCurrentTrack(0);
    }

    private void OnEnable()
    {
        if (_playing)
        {
            SetCurrentTrack(_currentTrackNumber);
        }
    }

    private void OnDestroy()
    {
        if (_emitter != null)
            Destroy(_emitter.gameObject);
        PlayerPrefs.SetFloat(VolumeSaveKey, volumeSlider.value);
        BehaviourUpdateUtils.Deregister(this);
        PdaMusicDatabase.OnMusicDatabaseChanged -= RefreshMusicDatabase;
    }
    
    public void ManagedUpdate()
    {
        if (!_playing) return;
        if (_emitter == null)
        {
            Plugin.Logger.LogError("No emitter assigned while music is playing!");
            return;
        }
        if (enabled)
            UpdateProgressPercent(Mathf.Clamp01(_emitter.time / _currentMusic.GetDuration()));
        if (_emitter.time >= _currentMusic.GetDuration() || !_emitter.isPlaying)
        {
            OnTrackFinish();
        }
    }

    private void RefreshMusicDatabase()
    {
        _currentPlaylist = new MusicPlaylist(PdaMusicDatabase.GetAllMusic());
    }

    private void SetCurrentTrack(int songIndex)
    {
        bool wasPlaying = _playing;
        
        _currentTrackNumber = songIndex;

        if (_currentPlaylist.Size == 0)
        {
            Plugin.Logger.LogWarning("No music found!");
            currentMusicText.text = "No music to play!";
            return;
        }

        var oldMusic = _currentMusic;
        _currentMusic = _currentPlaylist.Tracks[songIndex];
        bool isSameSong = oldMusic == _currentMusic;
        currentMusicText.text = $"{songIndex + 1}. {_currentMusic.GetTrackName()}";

        if (!isSameSong)
        {
            UpdateProgressPercent(0);
        
            StopOldTrackAndFadeOutIfNecessary();
        
            _emitter = new GameObject("PdaMusicEmitter").AddComponent<AudioSource>();
            _emitter.volume = volumeSlider.value * _currentMusic.VolumeMultiplier;
            _emitter.clip = _currentMusic.SoundAsset;
            
            SetMusicPlaying(wasPlaying);
        }
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

        if (_emitter == null)
        {
            _emitter = new GameObject("PdaMusicEmitter").AddComponent<AudioSource>();
            _emitter.volume = volumeSlider.value * _currentMusic.VolumeMultiplier;
            _emitter.clip = _currentMusic.SoundAsset;
        }

        if (newPlayingState)
        {
            _emitter.PlayDelayed(0.1f);
        }
        else
        {
            _emitter.Pause();
        }
    }

    private void SetLooping(bool looping)
    {
        _looping = looping;
        loopButtonImage.color = looping ? EnabledColor : Color.white;
    }

    private void SetShuffled(bool shuffled)
    {
        _shuffled = shuffled;
        shuffleButtonImage.color = _shuffled ? EnabledColor : Color.white;
        if (shuffled)
        {
            _currentPlaylist.Shuffle();
        }
        else
        {
            _currentPlaylist.ResetToDefaultOrder();
        }
        if (_currentPlaylist.TryGetTrackNumber(_currentMusic, out var equivalentTrackNumber))
            SetCurrentTrack(equivalentTrackNumber);
    }

    public void OnPlayButton()
    {
        SetMusicPlaying(!_playing);
    }

    public void OnNextButton()
    {
        SetCurrentTrack(ProperModulo(_currentTrackNumber + 1, _currentPlaylist.Size));
    }

    public void OnPreviousButton()
    {
        SetCurrentTrack(ProperModulo(_currentTrackNumber - 1, _currentPlaylist.Size));
    }

    public void OnLoopButton()
    {
        SetLooping(!_looping);
    }

    public void OnShuffleButton()
    {
        SetShuffled(!_shuffled);
    }

    public void OnProgressBarChanged(float newValue)
    {
        if (Mathf.Abs(newValue - _emitter.time / _currentMusic.GetDuration()) < Time.deltaTime * 1.5f)
            return;
        _emitter.time = newValue * _currentMusic.GetDuration();
    }
    
    public void OnVolumeSliderChanged(float newValue)
    {
        if (_emitter)
            _emitter.volume = newValue * (_currentMusic?.VolumeMultiplier ?? 1);
        volumeIndicatorImage.sprite = newValue > 0.01f ? soundOnSprite : soundOffSprite;
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

    private void OnTrackFinish()
    {
        if (_looping)
        {
            _emitter.Stop();
            _emitter.time = 0;
            _emitter.PlayDelayed(0.1f);
            return;
        }
        SetCurrentTrack(ProperModulo(_currentTrackNumber + 1, _currentPlaylist.Size));
    }

    public string GetProfileTag()
    {
        return "MusicPlayerUI";
    }

    public int managedUpdateIndex { get; set; }
}