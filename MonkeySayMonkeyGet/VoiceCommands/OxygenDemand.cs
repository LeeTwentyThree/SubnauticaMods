using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class OxygenDemand : VoiceCommandBase
{
    public override bool AllowPartialInput => true;

    public override Priority Priority => Priority.Low;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Oxygen);
    }

    protected override void Perform(SpeechInput input)
    {
        var amount = PhraseManager.GetReferencedNumber(input, 15);
        Player.main.oxygenMgr.AddOxygen(amount);
    }
 }