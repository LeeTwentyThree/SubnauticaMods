using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class Pain : VoiceCommandBase
{
    public override bool AllowPartialInput => true;

    public override Priority Priority => Priority.Medium;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Pain);
    }

    protected override void Perform(SpeechInput input)
    {
        Player.main.liveMixin.TakeDamage(10f);
    }
}