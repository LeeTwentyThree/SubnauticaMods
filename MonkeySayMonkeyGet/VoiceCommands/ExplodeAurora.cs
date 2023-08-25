using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class ExplodeAurora : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.High;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.ExplodeAurora);
    }

    protected override void Perform(SpeechInput input)
    {
        var ship = CrashedShipExploder.main;
        ship.timeToStartCountdown = ship.timeMonitor.Get() - 25f + 1f;
        ship.timeToStartWarning = ship.timeToStartCountdown - 1f;
    }
 }