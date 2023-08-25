using UnityEngine;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class Move : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.Medium;

    public override float MinimumDelay => 2f;

    protected override bool IsValid(SpeechInput input)
    {
        if (PhraseManager.ContainsPhrase(input, PhraseManager.Move))
        {
            var direction = PhraseManager.GetReferencedDirection(input);
            if (direction == Direction.None)
            {
                return false;
            }
            var subject = PhraseManager.GetSubject(input, true, Subject.Type.Self, true);
            if (subject.type != Subject.Type.None)
            {
                return true;
            }
        }
        return false;
    }

    protected override void Perform(SpeechInput input)
    {
        var amount = PhraseManager.GetReferencedNumber(input, 10);
        var direction = PhraseManager.GetReferencedDirection(input);
        activeSubject = PhraseManager.GetSubject(input, true, Subject.Type.Self, true);

        var player = Player.main;

        if (activeSubject.type == Subject.Type.Self)
        {
            if (player.GetCurrentSub() != null && player.IsPiloting())
            {
                activeSubject.gameObject = player.GetCurrentSub().gameObject;
            }
            if (player.GetVehicle() != null)
            {
                activeSubject.gameObject = player.GetVehicle().gameObject;
            }
        }

        if (StarPlatinum.StarPlatinumActivated)
        {
            if (direction == Direction.ToSelf)
            {
                var task = StarPlatinum.Instance.AssignTask<Mono.StarPlatinum.GrabEverything>();
                if (task)
                {
                    task.targetObjects = activeSubject.ToList(20);
                    task.bringToPosition = MainCameraControl.main.transform.position + MainCameraControl.main.transform.forward * 2f;
                    task.positionVariation = 2f;
                    return;
                }
            }
        }
        Transform aimingTransform = player.camRoot.GetAimingTransform();
        switch (direction)
        {
            default:
                break;
            case Direction.Forward:
                MovePosition(aimingTransform.forward * amount);
                break;
            case Direction.Back:
                MovePosition(-aimingTransform.forward * amount);
                break;
            case Direction.Left:
                MovePosition(-aimingTransform.right * amount);
                break;
            case Direction.Right:
                MovePosition(aimingTransform.right * amount);
                break;
            case Direction.Up:
                MovePosition(Vector3.up * amount);
                break;
            case Direction.Down:
                MovePosition(Vector3.down * amount);
                break;
            case Direction.Random:
                MovePosition(Random.onUnitSphere * amount);
                break;
            case Direction.Surface:
                Utils.SetSubjectYValue(activeSubject, 0f);
                break;
            case Direction.ToSelf:
                Utils.MoveSubjectToPosition(activeSubject, aimingTransform.position, 6f);
                break;
        }
    }

    private Subject activeSubject;

    private void MovePosition(Vector3 movement)
    {
        Utils.MoveSubjectInDirection(activeSubject, movement);
    }
}