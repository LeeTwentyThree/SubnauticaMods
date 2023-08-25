namespace MonkeySayMonkeyGet.VoiceCommands;

public class Water : SpawnAroundPlayerCommand
{
    public override string ClassId => "bf9ccd04-60af-4144-aaa1-4ac184c686c2";

    public override int MinSpawns => 16;

    public override int MaxSpawns => 24;

    public override float SpawnRadius => 4f;

    public override Priority Priority => Priority.None;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Water);
    }
}
