using UnityEngine;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class GotoHome : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.Low;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Home);
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
            return;
        }
        player.lastEscapePod?.RespawnPlayer();
        Player.main.currentEscapePod = Player.main.lastEscapePod;
        Player.main.SetPrecursorOutOfWater(false);
    }
}