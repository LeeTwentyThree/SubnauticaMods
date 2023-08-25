namespace MonkeySayMonkeyGet.VoiceCommands;

public class Day : VoiceCommandBase
{
    public override bool AllowPartialInput => true;

    public override Priority Priority => Priority.Low;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Day);
    }

    protected override void Perform(SpeechInput input)
    {
        var dnc = DayNightCycle.main;
        bool flag = dnc.IsDay();
        dnc.timePassedAsDouble += 1200.0 - dnc.timePassed % 1200.0 + 600.0;
        dnc.skipTimeMode = false;
        dnc._dayNightSpeed = 1f;
        dnc.UpdateAtmosphere();
        if (!flag)
        {
            dnc.dayNightCycleChangedEvent.Trigger(true);
        }
    }

    public override float MinimumDelay => 10f;
}