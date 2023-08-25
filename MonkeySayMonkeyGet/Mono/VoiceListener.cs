using UnityEngine;
using System.Collections.Generic;
using MonkeySayMonkeyGet.VoiceCommands;
using MonkeySayMonkeyGet.SpeechRecognition;

namespace MonkeySayMonkeyGet.Mono;

public class VoiceListener : MonoBehaviour
{
    public static VoiceListener main;

    public SpeechRecognizer speechRecognizer;
    public VoiceCommandBase lastPerformed;
    public SpeechInput lastFedInput;
    public Vector3 positionBeforeWarp;

    private List<VoiceCommandBase> voiceCommands;

    private bool[] validVoiceCommands;

    private void Awake()
    {
        main = this;
        InitializeVoiceCommands();
    }

    // most important thing in the mod

    private void InitializeVoiceCommands()
    {
        voiceCommands = new List<VoiceCommandBase>();
        voiceCommands.Add(new Hello());
        voiceCommands.Add(new Die());
        voiceCommands.Add(new Food());
        voiceCommands.Add(new Water());
        voiceCommands.Add(new OxygenDemand());
        voiceCommands.Add(new StopTime());
        voiceCommands.Add(new Again());
        voiceCommands.Add(new Day());
        voiceCommands.Add(new Night());
        voiceCommands.Add(new GotoHome());
        voiceCommands.Add(new GotoLifepod());
        voiceCommands.Add(new GotoSubmarine());
        voiceCommands.Add(new Move());
        voiceCommands.Add(new Pain());
        voiceCommands.Add(new SwimSpeed());
        voiceCommands.Add(new GoToPOI());
        voiceCommands.Add(new Drown());
        voiceCommands.Add(new Kill());
        voiceCommands.Add(new BeatGame());
        voiceCommands.Add(new Create());
        voiceCommands.Add(new Heal());
        voiceCommands.Add(new Save());
        voiceCommands.Add(new Cyclops());
        voiceCommands.Add(new Blueprint());
        voiceCommands.Add(new Delete());
        voiceCommands.Add(new Explore());
        voiceCommands.Add(new VoiceCommands.StarPlatinum());
        voiceCommands.Add(new ChangeSize());
        voiceCommands.Add(new Jump());
        voiceCommands.Add(new Fly());
        voiceCommands.Add(new PlaySound());
        voiceCommands.Add(new Talk());
        voiceCommands.Add(new Vomit());
        voiceCommands.Add(new Starve());
        voiceCommands.Add(new Stuck());
        voiceCommands.Add(new ExplodeAurora());

        validVoiceCommands = new bool[voiceCommands.Count];
    }

    public static void CreateInstance()
    {
        var go = new GameObject("VoiceListener");
        var vl = go.AddComponent<VoiceListener>();

        var srGo = new GameObject("SpeechRecognizer");
        srGo.transform.SetParent(go.transform, false);
        var speechRecognizer = srGo.AddComponent<SpeechRecognizer>();
        vl.speechRecognizer = speechRecognizer;

        var arGo = new GameObject("AudioRecorder");
        arGo.transform.SetParent(go.transform, false);
        var audioRecorder = arGo.AddComponent<AudioRecorder>();
        audioRecorder.MicReady.AddListener(speechRecognizer.OnDataProviderReady);
    }

    private void Start()
    {
        speechRecognizer.ResultReceived.AddListener(OnReceiveFullMessage);
        speechRecognizer.PartialResultReceived.AddListener(OnReceivePartialMessage);
    }

    public void OnReceiveFullMessage(string message)
    {
        OnReceiveMessageInternal(new SpeechInput(message, SpeechInput.Context.Final));
    }

    public void OnReceivePartialMessage(string message)
    {
        OnReceiveMessageInternal(new SpeechInput(message, SpeechInput.Context.Partial));
    }

    private void OnReceiveMessageInternal(SpeechInput message)
    {
        for (var i = 0; i < voiceCommands.Count; i++)
        {
            validVoiceCommands[i] = voiceCommands[i].TestInputValid(message);
        }
        var selectedPriority = int.MinValue;
        var selectedIndex = -1;
        for (var i = 0; i < voiceCommands.Count; i++)
        {
            if (validVoiceCommands[i] == true)
            {
                var priority = (int)voiceCommands[i].Priority;
                if (priority > selectedPriority)
                {
                    selectedPriority = priority;
                    selectedIndex = i;
                }
            }
        }
        if (selectedIndex >= 0)
        {
            voiceCommands[selectedIndex].FeedInput(message);
            if (voiceCommands[selectedIndex].GetType() != typeof(Again))
            {
                lastPerformed = voiceCommands[selectedIndex];
                lastFedInput = message;
            }
        }
    }
}