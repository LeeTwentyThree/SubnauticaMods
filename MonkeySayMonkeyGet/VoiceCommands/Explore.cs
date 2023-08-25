using UnityEngine;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class Explore : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.Low;

    protected override bool IsValid(SpeechInput input)
    {
        return PhraseManager.ContainsPhrase(input, PhraseManager.Explore);
    }

    protected override void Perform(SpeechInput input)
    {
        var player = Player.main;

        activeSubject = new Subject(Subject.Type.Self);
        if (player.GetCurrentSub() != null && player.IsPiloting())
        {
            activeSubject.gameObject = player.GetCurrentSub().gameObject;
        }
        if (player.GetVehicle() != null)
        {
            activeSubject.gameObject = player.GetVehicle().gameObject;
        }
        var randomUnit = Random.insideUnitCircle;
        var randomInWorld = randomUnit * Random.Range(1f, 2000f);
        Utils.MoveSubjectToPosition(activeSubject, new Vector3(randomInWorld.x, 0f, randomInWorld.y));
    }

    private Subject activeSubject;
}