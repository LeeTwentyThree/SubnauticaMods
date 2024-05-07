using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheRedPlague.Mono.Tools;

public class PlagueKnifeTool : Knife
{
    public override string animToolName => "knife";

    public override void OnToolUseAnim(GUIHand hand)
    {
        Vector3 position = default(Vector3);
        GameObject closestObj = null;
        UWE.Utils.TraceFPSTargetPosition(Player.main.gameObject, attackDist, ref closestObj, ref position);
        if (position == default)
            position = MainCamera.camera.transform.position + MainCamera.camera.transform.forward * attackDist;
        if (closestObj == null)
        {
            InteractionVolumeUser component = Player.main.gameObject.GetComponent<InteractionVolumeUser>();
            if (component != null && component.GetMostRecent() != null)
            {
                closestObj = component.GetMostRecent().gameObject;
            }
        }
        if ((bool)closestObj)
        {
            LiveMixin liveMixin = closestObj.FindAncestor<LiveMixin>();
            if (IsValidTarget(liveMixin))
            {
                if ((bool)liveMixin)
                {
                    bool wasAlive = liveMixin.IsAlive();
                    liveMixin.TakeDamage(damage, position, damageType);
                    GiveResourceOnDamage(closestObj, liveMixin.IsAlive(), wasAlive);
                }
                Utils.PlayFMODAsset(attackSound, base.transform);
                VFXSurface component2 = closestObj.GetComponent<VFXSurface>();
                Vector3 euler = MainCameraControl.main.transform.eulerAngles + new Vector3(300f, 90f, 0f);
                VFXSurfaceTypeManager.main.Play(component2, vfxEventType, position, Quaternion.Euler(euler), Player.main.transform);
            }
            else
            {
                closestObj = null;
            }
        }
        if (closestObj == null && hand.GetActiveTarget() == null)
        {
            if (Player.main.IsUnderwater())
            {
                Utils.PlayFMODAsset(underwaterMissSound, transform);
            }
            else
            {
                Utils.PlayFMODAsset(surfaceMissSound, transform);
            }
        }

        StartCoroutine(SpawnWarper(position));
    }

    private IEnumerator SpawnWarper(Vector3 pos)
    {
        var warperTask = CraftData.GetPrefabForTechTypeAsync(TechType.Warper);
        yield return warperTask;
        var offsetPosition = pos + Random.onUnitSphere * 0.5f;
        var warperObj = Instantiate(warperTask.GetResult(), offsetPosition, Quaternion.LookRotation((offsetPosition - pos).normalized));
        warperObj.AddComponent<FriendlyWarper>();
        warperObj.SetActive(true);
        var warper = warperObj.GetComponent<Warper>();
        warper.SetFriend(Player.main.gameObject);
        var colliders = Physics.OverlapSphere(pos, 5, -1, QueryTriggerInteraction.Ignore);
        var warperMeleeAttack = warperObj.GetComponentInChildren<WarperMeleeAttack>();
        warperMeleeAttack.attackDamage = new AnimationCurve(new Keyframe(0, 100), new Keyframe(4, 100), new Keyframe(2, 100));
        var attackedTargets = new List<Creature>();
        foreach (var target in colliders)
        {
            if (target == null)
                continue;
            var creatureComponent = target.GetComponentInParent<Creature>();
            if (creatureComponent == null || creatureComponent == warper)
                continue;
            if (attackedTargets.Contains(creatureComponent))
                continue;
            attackedTargets.Add(creatureComponent);
            
            if (warper == null) yield break;
            warper.transform.LookAt(creatureComponent.transform);
            warperMeleeAttack.timeLastBite = 0;
            warper.Aggression.Add(1);
            warperMeleeAttack.OnTouch(creatureComponent.GetComponent<Collider>());
            
            yield return new WaitForSeconds(0.5f);
            yield return null;
        }
        warper.WarpOut();
    }
}