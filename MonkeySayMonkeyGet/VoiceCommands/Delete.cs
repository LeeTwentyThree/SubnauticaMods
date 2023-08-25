using UnityEngine;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class Delete : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.Medium;

    protected override bool IsValid(SpeechInput input)
    {
        if (!PhraseManager.ContainsPhrase(input, PhraseManager.Delete))
        {
            return false;
        }
        var mentionedEverything = Plugin.config.CanMentionEverything && PhraseManager.ContainsPhrase(input, PhraseManager.Everything);
        if (mentionedEverything)
        {
            return true;
        }
        if (!PhraseManager.ContainsPhrase(input, PhraseManager.PronounPlural) && !PhraseManager.ContainsPhrase(input, PhraseManager.Pronoun) && !PhraseManager.ContainsPhrase(input, PhraseManager.PronounEvery))
        {
            return false;
        }
        var referencedTechType = PhraseManager.GetReferencedTechType(input, out Amount _);
        return referencedTechType != TechType.None;
    }

    protected override void Perform(SpeechInput input)
    {
        var objectsMentioned = Utils.FindReferencedObjects(input);
        foreach (var obj in objectsMentioned)
        {
            Object.Destroy(obj);
        }
    }

    public override float MinimumDelay => 2f;
}