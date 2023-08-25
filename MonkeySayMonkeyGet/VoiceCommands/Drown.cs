using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class Drown : VoiceCommandBase
{
    public override bool AllowPartialInput => true;

    public override Priority Priority => Priority.Low;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Drown);
    }

    protected override void Perform(SpeechInput input)
    {
        Player.main.oxygenMgr.RemoveOxygen(180f);
    }
 }