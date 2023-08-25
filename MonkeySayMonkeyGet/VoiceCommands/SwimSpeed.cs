namespace MonkeySayMonkeyGet.VoiceCommands;

public class SwimSpeed : VoiceCommandBase
{
    public override bool AllowPartialInput => true;

    public override Priority Priority => Priority.Low;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Speed);
    }

    protected override void Perform(SpeechInput input)
    {
        var speedChange = PhraseManager.GetReferencedSpeedChange(input);
        if (speedChange == SpeedChange.Slower)
        {
            currentSpeed = (Speed)((int)currentSpeed - 1);
        }
        if (speedChange == SpeedChange.Faster)
        {
            currentSpeed = (Speed)((int)currentSpeed + 1);
        }
        if (speedChange == SpeedChange.Reset)
        {
            currentSpeed = Speed.Default;
        }
        var speedInteger = (int)currentSpeed;
        var speedMultiplier = 1f + (speedInteger / 3f);
        var player = Player.main;
        var swimMotor = player.GetComponent<UnderwaterMotor>();
        swimMotor.waterAcceleration = defaultSwimAcceleration * speedMultiplier;
        swimMotor.forwardMaxSpeed = defaultSwimSpeed * speedMultiplier;
        swimMotor.strafeMaxSpeed = defaultSwimSpeed * speedMultiplier;
        swimMotor.verticalMaxSpeed = defaultSwimSpeed * speedMultiplier;
        var groundMotor = player.GetComponent<GroundMotor>();
        groundMotor.groundAcceleration = defaultWalkAcceleration * speedMultiplier;
        groundMotor.forwardMaxSpeed = defaultWalkSpeed * speedMultiplier;
    }

    public override float MinimumDelay => 1f;

    private Speed currentSpeed = Speed.Default;

    private float defaultSwimAcceleration = 20f;
    private float defaultSwimSpeed = 5f;

    private float defaultWalkSpeed = 3.5f;
    private float defaultWalkAcceleration = 45f;

    public enum Speed
    {
        Slower = -2,
        Slow = -1,
        Default = 0,
        Fast = 1,
        Faster = 2
    }
}