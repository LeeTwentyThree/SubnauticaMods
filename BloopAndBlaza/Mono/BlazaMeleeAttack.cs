using ECCLibrary;
using UnityEngine;

namespace BloopAndBlaza.Mono;

public class BlazaMeleeAttack : MeleeAttack
{
    public FMOD_CustomEmitter attackEmitter;
    
    void Start()
    {
        gameObject.SearchChild("Mouth").GetComponent<OnTouch>().onTouch = new OnTouch.OnTouchEvent();
        gameObject.SearchChild("Mouth").GetComponent<OnTouch>().onTouch.AddListener(OnTouch);
    }

    public override void OnTouch(Collider collider)
    {
        if (frozen)
        {
            return;
        }

        if (liveMixin.IsAlive() && Time.time > timeLastBite + biteInterval)
        {
            Creature component = GetComponent<Creature>();
            if (component.Aggression.Value >= 0.1f)
            {
                GameObject target = GetTarget(collider);
                BlazaBehaviour blazaBehaviour = GetComponent<BlazaBehaviour>();
                if (!blazaBehaviour.IsHoldingVehicle())
                {
                    Player player = target.GetComponent<Player>();
                    if (player != null)
                    {
                        if (!player.CanBeAttacked() || !player.liveMixin.IsAlive() || player.cinematicModeActive)
                        {
                            return;
                        }
                    }
                    else if (blazaBehaviour.GetCanGrabVehicle())
                    {
                        SeaMoth component4 = target.GetComponent<SeaMoth>();
                        if (component4 && !component4.docked)
                        {
                            blazaBehaviour.GrabGenericSub(component4);
                            component.Aggression.Value -= 0.25f;
                            return;
                        }

                        Exosuit component5 = target.GetComponent<Exosuit>();
                        if (component5 && !component5.docked)
                        {
                            blazaBehaviour.GrabExosuit(component5);
                            component.Aggression.Value -= 0.25f;
                            return;
                        }
                    }

                    LiveMixin liveMixin = target.GetComponent<LiveMixin>();
                    if (liveMixin == null) return;
                    if (!liveMixin.IsAlive())
                    {
                        return;
                    }

                    if (!CanAttackTargetFromPosition(target))
                    {
                        return;
                    }

                    liveMixin.TakeDamage(GetBiteDamage(target));
                    timeLastBite = Time.time;
                    attackEmitter.Play();

                    creature.GetAnimator().SetTrigger("bite");
                    component.Aggression.Value -= 0.15f;
                }
            }
        }
    }

    private bool CanAttackTargetFromPosition(GameObject target)
    {
        Vector3 direction = target.transform.position - transform.position;
        float magnitude = direction.magnitude;
        int num = UWE.Utils.RaycastIntoSharedBuffer(transform.position, direction, magnitude, -5,
            QueryTriggerInteraction.Ignore);
        for (int i = 0; i < num; i++)
        {
            Collider collider = UWE.Utils.sharedHitBuffer[i].collider;
            GameObject gameObject = (collider.attachedRigidbody != null)
                ? collider.attachedRigidbody.gameObject
                : collider.gameObject;
            if (!(gameObject == target) && !(gameObject == base.gameObject) &&
                !(gameObject.GetComponent<Creature>() != null))
            {
                return false;
            }
        }

        return true;
    }

    private float GetBiteDamage(GameObject target)
    {
        if (target.GetComponent<SubControl>() != null)
        {
            return 300f; //cyclops damage
        }

        if (target.GetComponent<Creature>() != null)
        {
            return 300f;
        }

        return biteDamage; //base damage
    }

    public void OnVehicleReleased()
    {
        timeLastBite = Time.time;
    }
}