using UnityEngine;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class Jump : VoiceCommandBase
{
    public override bool AllowPartialInput => true;

    public override Priority Priority => Priority.Low;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Jump);
    }

    protected override void Perform(SpeechInput input)
    {
        if (Player.main.IsSwimming())
        {
            Player.main.rigidBody.AddForce(Vector3.up * 5f, ForceMode.VelocityChange);
        }
        else
        {
            var groundMotor = Player.main.groundMotor;
            groundMotor.jumpPressed = true;
            groundMotor.jumping.holdingJumpButton = true;
        }
    }
}