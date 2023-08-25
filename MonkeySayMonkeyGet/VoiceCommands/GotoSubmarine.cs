using UnityEngine;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class GotoSubmarine : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.Low;

    protected override bool IsValid(SpeechInput input)
    {
        if (Player.main.GetCurrentSub() != null)
        {
            return false;
        }
        return PhraseManager.ContainsPhrase(input, PhraseManager.Submarine);
    }

    protected override void Perform(SpeechInput input)
    {
        var player = Player.main;
        if (player.lastValidSub != null)
        {
            RespawnPoint componentInChildren2 = player.lastValidSub.gameObject.GetComponentInChildren<RespawnPoint>();
            player.SetPosition(componentInChildren2.GetSpawnPosition());
            player.SetCurrentSub(player.lastValidSub);
            Player.main.SetPrecursorOutOfWater(false);
        }
    }
}