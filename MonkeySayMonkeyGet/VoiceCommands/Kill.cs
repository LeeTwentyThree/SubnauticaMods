using UnityEngine;
using MonkeySayMonkeyGet.Mono.StarPlatinum;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class Kill : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.High;

    protected override bool IsValid(SpeechInput input)
    {
        if (!PhraseManager.ContainsPhrase(input, PhraseManager.Kill))
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
        var maxDistance = float.MaxValue;
        if (StarPlatinum.StarPlatinumActivated)
        {
            maxDistance = 40f;
        }
        var objectsMentioned = Utils.FindReferencedObjects(input, maxDistance);
        if (StarPlatinum.StarPlatinumActivated)
        {
            var instance = StarPlatinum.Instance;
            if (objectsMentioned.Count > 1)
            {
                var task = instance.AssignTask<KillList>();
                if (task)
                {
                    task.targetObjects = objectsMentioned;
                    return;
                }
            }
            else
            {
                var task = instance.AssignTask<KillSmall>();
                if (task)
                {
                    task.targetObject = objectsMentioned[0];
                    return;
                }
            }
        }
        foreach (var obj in objectsMentioned)
        {
            var lm = obj.GetComponent<LiveMixin>();
            if (lm)
            {
                lm.TakeDamage(10000f);
            }
            var breakableResource = obj.GetComponent<BreakableResource>();
            if (breakableResource)
            {
                breakableResource.BreakIntoResources();
            }
        }
    }

    public override float MinimumDelay => 2f;
}