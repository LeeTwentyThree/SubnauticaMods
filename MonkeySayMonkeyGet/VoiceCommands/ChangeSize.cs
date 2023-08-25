using UnityEngine;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class ChangeSize : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.Medium;

    protected override bool IsValid(SpeechInput input)
    {
        var sizeChange = PhraseManager.GetReferencedScaleChange(input);
        if (sizeChange == ScaleChange.None)
        {
            return false;
        }
        var mentionedEverything = Plugin.config.CanMentionEverything && PhraseManager.ContainsPhrase(input, PhraseManager.Everything);
        if (mentionedEverything)
        {
            return true;
        }
        var referencedTechType = PhraseManager.GetReferencedTechType(input, out Amount _);
        return referencedTechType != TechType.None;
    }

    protected override void Perform(SpeechInput input)
    {
        var sizeChange = PhraseManager.GetReferencedScaleChange(input);
        var objectsMentioned = Utils.FindReferencedObjects(input);
        foreach (var obj in objectsMentioned)
        {
            var transform = obj.transform;
            var defaultScale = transform.localScale;
            var zScale = transform.localScale.z;
            switch (sizeChange)
            {
                default:
                    break;
                case ScaleChange.Shorter:
                    transform.localScale = new Vector3(defaultScale.x, defaultScale.y, zScale * 0.5f);
                    break;
                case ScaleChange.Longer:
                    transform.localScale = new Vector3(defaultScale.x, defaultScale.y, zScale * 2f);
                    break;
                case ScaleChange.Bigger:
                    transform.localScale = defaultScale * 2f;
                    break;
                case ScaleChange.Smaller:
                    transform.localScale = defaultScale * 0.5f;
                    break;
            }
        }
    }

    public override float MinimumDelay => 2f;
}