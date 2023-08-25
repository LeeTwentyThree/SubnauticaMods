using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class Starve : VoiceCommandBase
{
    public override bool AllowPartialInput => true;

    public override Priority Priority => Priority.Low;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Starve);
    }

    protected override void Perform(SpeechInput input)
    {
        var survival = Player.main.GetComponent<Survival>();
        survival.food = 0f;
        survival.UpdateHunger();
    }
 }