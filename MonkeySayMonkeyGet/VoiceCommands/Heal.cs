using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class Heal : VoiceCommandBase
{
    public override bool AllowPartialInput => true;

    public override Priority Priority => Priority.Low;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Heal);
    }

    protected override void Perform(SpeechInput input)
    {
        var amount = PhraseManager.GetReferencedNumber(input, 20);
        Player.main.liveMixin.AddHealth(amount);
    }
}