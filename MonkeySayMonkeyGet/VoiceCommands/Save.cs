using UWE;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class Save : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.None;

    public override float MinimumDelay => 15f;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Save);
    }

    protected override void Perform(SpeechInput input)
    {
        IngameMenu.main.SaveGame();
    }
}