using MonkeySayMonkeyGet.Mono.StarPlatinum;
using System.Collections;
using UnityEngine;
using UWE;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class StarPlatinum : VoiceCommandBase
{
    public override bool AllowPartialInput => true;

    public override Priority Priority => Priority.Low;

    protected override bool IsValid(SpeechInput input)
    {
        if (StarPlatinumActivated)
        {
            if (PhraseManager.ContainsPhrase(input, PhraseManager.Cancel))
            {
                return true;
            }
        }
        return PhraseManager.ContainsPhrase(input, PhraseManager.StarPlatinum);
    }

    protected override void Perform(SpeechInput input)
    {
        timeLastActivated = Time.time;
        CoroutineHost.StartCoroutine(PerformAsync());
    }

    private static IEnumerator PerformAsync()
    {
        if (!StarPlatinumActivated)
        {
            var result = new TaskResult<StarPlatinumInstance>();
            yield return StarPlatinumInstance.CreateInstance(result);
            _instance = result.Get();
        }
        _instance.MoveToPlayer();
        _instance.CancelAllTasks();
    }

    public override float MinimumDelay => 3f;

    public static float timeLastActivated = -999f;

    public static readonly float duration = 60f;

    public static bool StarPlatinumActivated
    {
        get
        {
            return Time.time < timeLastActivated + duration && Instance != null;
        }
    }

    private static StarPlatinumInstance _instance;

    public static StarPlatinumInstance Instance
    {
        get
        {
            return _instance;
        }
    }
}