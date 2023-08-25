using UnityEngine;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class Blueprint : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.High;

    protected override bool IsValid(SpeechInput input)
    {
        if (!PhraseManager.ContainsPhrase(input, PhraseManager.Blueprint))
        {
            return false;
        }
        var mentionedTechType = PhraseManager.GetReferencedTechType(input, out var _);
        if (mentionedTechType == TechType.None)
        {
            return false;
        }
        return true;
    }

    protected override void Perform(SpeechInput input)
    {
        var mentionedTechType = PhraseManager.GetReferencedTechType(input, out var _);
        KnownTech.Add(mentionedTechType);
    }
 }