namespace MonkeySayMonkeyGet.VoiceCommands;

public class Food : SpawnAroundPlayerCommand
{
    public override string ClassId => "3fcd548b-781f-46ba-b076-7412608deeef";

    public override int MinSpawns => 16;

    public override int MaxSpawns => 24;

    public override float SpawnRadius => 4f;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Food);
    }
}
