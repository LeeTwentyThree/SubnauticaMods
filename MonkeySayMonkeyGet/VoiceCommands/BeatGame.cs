namespace MonkeySayMonkeyGet.VoiceCommands;

public class BeatGame : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.Low;

    protected override bool IsValid(SpeechInput input)
    {
        if (Plugin.ModConfig.DisableBeatTheGame)
        {
            return false;
        }
        return PhraseManager.ContainsPhrase(input, PhraseManager.BeatGame);
    }

    protected override void Perform(SpeechInput input)
    {
        Utils.PlayCredits();
    }

    public override float MinimumDelay => 10f;
}