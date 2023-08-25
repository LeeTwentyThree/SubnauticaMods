using UnityEngine;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class GoToPOI : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.Medium;

    protected override bool IsValid(SpeechInput input)
    {
        var poi = PhraseManager.GetReferencedPOI(input);
        return poi.IsValid;
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
        var poi = PhraseManager.GetReferencedPOI(input);
        if (!poi.IsValid)
        {
            return;
        }
        Utils.MoveSubjectToPosition(activeSubject, poi.coords);

    }

    private Subject activeSubject;
}