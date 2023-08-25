using UnityEngine;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class Cyclops : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.Medium;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Demand) && PhraseManager.ContainsPhrase(input, PhraseManager.Cyclops);
    }

    protected override void Perform(SpeechInput input)
    {
        var transform = MainCamera.camera.transform;
        var spawnPosition = transform.position + 20f * transform.forward;
        var spawnRotation = Quaternion.LookRotation(MainCamera.camera.transform.right);
        SubConsoleCommand.main.SpawnSub("cyclops", spawnPosition, spawnRotation);
    }
 }