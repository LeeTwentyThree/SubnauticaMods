using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

using SpeechRecognitionSystem;

using UnityEngine;
using UnityEngine.Events;

namespace MonkeySayMonkeyGet.SpeechRecognition;

public class SpeechRecognizer : MonoBehaviour
{
    public void OnDataProviderReady(IAudioProvider audioProvider)
    {
        _audioProvider = audioProvider;
        _running = true;
        Task.Run(processing).ConfigureAwait(false);
    }

    private readonly ConcurrentQueue<float[]> _threadedBufferQueue = new ConcurrentQueue<float[]>();
    private readonly ConcurrentQueue<string> _recognitionPartialResultsQueue = new ConcurrentQueue<string>();
    private readonly ConcurrentQueue<string> _recognitionFinalResultsQueue = new ConcurrentQueue<string>();

    [Serializable]
    public class MessageEvent : UnityEvent<string> { }

    public MessageEvent LogMessageReceived = new MessageEvent();
    public MessageEvent PartialResultReceived = new MessageEvent();
    public MessageEvent ResultReceived = new MessageEvent();

    private void onInitResult(string modelDirPath)
    {
        if (modelDirPath != string.Empty)
        {
            if (Directory.Exists(modelDirPath))
            {
                _init = _sr.Init(modelDirPath);
            }
            else
            {
                LogMessageReceived?.Invoke("$directorynull");
                _init = false;
            }
            if (!_init)
            {
                LogMessageReceived?.Invoke("$initerror");
            }
        }
    }

    private void onReceiveLogMess(string message)
    {
        LogMessageReceived?.Invoke(message);
    }

    private void Awake()
    {
        _sr = new SpeechRecognitionSystem.SpeechRecognizer();
    }

    // initialize the external plugin
    private void Start()
    {
        LogMessageReceived.AddListener(_errorHandler.ReceiveMessage);
        ResultReceived.AddListener(_testingHandler.ReceiveMessage);
        var path = Path.Combine(Directory.GetParent(Plugin.Assembly.Location).FullName, Plugin.LanguageModelDirPath).Replace('\\', '/'); // just to be safe... i'm afraid the unmanaged assembly dislikes backslashes
        onInitResult(path);
    }

    private void FixedUpdate()
    {
        if (_init && (_audioProvider != null))
        {
            var audioData = _audioProvider.GetData();
            if (audioData != null)
                _threadedBufferQueue.Enqueue(audioData);

            if (_recognitionPartialResultsQueue.TryDequeue(out string part))
            {
                if (part != string.Empty)
                    PartialResultReceived?.Invoke(part);
            }
            if (_recognitionFinalResultsQueue.TryDequeue(out string result))
            {
                if (result != string.Empty)
                    ResultReceived?.Invoke(result);
            }
        }
    }

    private async Task processing()
    {
        while (_running)
        {
            float[] audioData;
            var isOk = _threadedBufferQueue.TryDequeue(out audioData);
            if (isOk)
            {
                int resultReady = _sr.AppendAudioData(audioData);
                if (resultReady == 0)
                {
                    _recognitionPartialResultsQueue.Enqueue(_sr.GetPartialResult()?.partial);
                }
                else
                {
                    _recognitionFinalResultsQueue.Enqueue(_sr.GetResult()?.text);
                }
            }
            else
            {
                await Task.Delay(100);
            }
        }
    }

    private bool _running = false;

    private void OnDestroy()
    {
        _init = false;
        _copyRequested = false;
        _running = false;
        _sr.Dispose();
        _sr = null;
    }

    private void copyAssets2ExternalStorage(string modelDirPath)
    {

    }

    private SpeechRecognitionSystem.SpeechRecognizer _sr = null;
    private IAudioProvider _audioProvider = null;
    private bool _init = false;
    private bool _copyRequested = false;
    private ErrorHandler _errorHandler = new ErrorHandler();
    private TestingHandler _testingHandler = new TestingHandler();

    public class ErrorHandler
    {
        private Dictionary<string, string> errorTexts = new Dictionary<string, string>() { { "$initerror", "Failed to load speech recognition model!" }, { "$directorynull", "Given directory does not exist!" } };

        public void ReceiveMessage(string message)
        {
            if (errorTexts.TryGetValue(message, out var text))
            {
                var formatted = string.Format("MonkeySayMonkeyDo ERROR: {0}", text);
                ErrorMessage.AddMessage(formatted);
                Debug.LogError(formatted);
            }
        }
    }

    public class TestingHandler
    {
        public void ReceiveMessage(string message)
        {
            if (Plugin.TestingModeActive)
            {
                ErrorMessage.AddMessage(message);
                Debug.LogError(message);
            }
        }
    }
}