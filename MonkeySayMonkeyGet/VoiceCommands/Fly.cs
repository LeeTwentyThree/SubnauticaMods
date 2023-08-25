using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class Fly : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.Medium;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Fly);
    }

    protected override void Perform(SpeechInput input)
    {
        if (!Player.main.IsSwimming())
        {
            var groundMotor = Player.main.groundMotor;
            groundMotor.jumpPressed = true;
            groundMotor.jumping.holdingJumpButton = true;
        }
        var gm = Player.main.groundMotor;
        gm.allowMidAirJumping = !gm.allowMidAirJumping;
    }
}