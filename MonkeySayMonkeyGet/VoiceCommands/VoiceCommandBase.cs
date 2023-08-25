using UnityEngine;

namespace MonkeySayMonkeyGet.VoiceCommands;

public abstract class VoiceCommandBase
{
    public bool TestInputValid(SpeechInput input)
    {
        if (input.context == SpeechInput.Context.Partial && !AllowPartialInput)
        {
            return false;
        }
        if (input.context == SpeechInput.Context.Final && IgnoreFullInput)
        {
            return false;
        }
        if (Time.time < timeCanPerformAgain)
        {
            return false;
        }
        if (Time.time < globalTimeCanPerformAgain)
        {
            return false;
        }
        return IsValid(input);
    }

    public void FeedInput(SpeechInput input)
    {
        if (Plugin.TestingModeActive)
        {
            var message = $"Performing {GetType().Name}";
            ErrorMessage.AddMessage(message);
            Debug.LogError(message);
        }
        Perform(input);
        timeCanPerformAgain = Time.time + MinimumDelay;
        globalTimeCanPerformAgain = Time.time + globalDelay;
    }

    protected abstract bool IsValid(SpeechInput input);

    protected abstract void Perform(SpeechInput input);

    public abstract bool AllowPartialInput { get; }

    public abstract Priority Priority { get; }

    public virtual float MinimumDelay => 3f;

    public virtual bool IgnoreFullInput => false;

    private float timeCanPerformAgain;

    private static float globalTimeCanPerformAgain;

    private static float globalDelay = 0.4f;
}
