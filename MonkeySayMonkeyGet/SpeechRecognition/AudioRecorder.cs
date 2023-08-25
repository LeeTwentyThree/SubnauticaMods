using UnityEngine;
using SpeechRecognitionSystem;

namespace MonkeySayMonkeyGet.SpeechRecognition;

public class AudioRecorder : MonoBehaviour, IAudioProvider
{
    public int MicrophoneIndex = 0;
    public int GetRecordPosition()
    {
        return Microphone.GetPosition(_deviceName);
    }
    public AudioClip GetAudioClip()
    {
        return _audioClip;
    }
    public bool IsRecording()
    {
        return Microphone.IsRecording(_deviceName);
    }

    public float[] GetData()
    {
        int pos = Microphone.GetPosition(_deviceName);
        int diff = pos - _lastSample;
        if (diff > 0)
        {
            var samples = new float[diff];
            _audioClip.GetData(samples, _lastSample);
            _lastSample = pos;
            return samples;
        }
        _lastSample = pos;
        return null;
    }

    public AudioReadyEvent MicReady = new AudioReadyEvent();

    private void Update()
    {
        bool micAutorized = true;
        if (micAutorized)
        {
            if (_firstLoad)
            {
                _deviceName = Microphone.devices[MicrophoneIndex];
                _audioClip = Microphone.Start(_deviceName, true, LENGTH_SEC, FREQ);
                this.MicReady?.Invoke(this);
                _firstLoad = false;
            }
        }
    }
    private void OnDestroy()
    {
        Microphone.End(_deviceName);
        _firstLoad = true;
    }

    private bool _firstLoad = true;
    private AudioClip _audioClip = null;
    private const int LENGTH_SEC = 2;
    private const int FREQ = 16000;
    private string _deviceName;

    private int _currentPosition = 0;

    private bool _init = false;

    private const int FRAME_LENGTH = 512;

    private int _lastSample = 0;
}