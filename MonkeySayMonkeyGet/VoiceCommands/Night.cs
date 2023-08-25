namespace MonkeySayMonkeyGet.VoiceCommands;

public class Night : VoiceCommandBase
{
    public override bool AllowPartialInput => true;

    public override Priority Priority => Priority.Low;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Night);
    }

    protected override void Perform(SpeechInput input)
    {
        var dnc = DayNightCycle.main;
        bool flag = dnc.IsDay();
        dnc.timePassedAsDouble += 1200.0 - dnc.timePassed % 1200.0;
        dnc.skipTimeMode = false;
        dnc._dayNightSpeed = 1f;
        dnc.UpdateAtmosphere();
        if (flag)
        {
            dnc.dayNightCycleChangedEvent.Trigger(false);
        }
    }

    public override float MinimumDelay => 10f;
}