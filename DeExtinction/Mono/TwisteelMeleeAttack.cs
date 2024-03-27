using Nautilus.Utility;
using UnityEngine;

namespace DeExtinction.Mono;

// idk why this class is so ugly, tbh... not worth redoing for now unless we notice any major issues
public class TwisteelMeleeAttack : MeleeAttack
{
    public FMOD_CustomEmitter attackEmitter;
    public Transform playerCameraAnimatedTransform;
    private static FMODAsset _biteSound = AudioUtils.GetFmodAsset("TwisteelAttack");
    private static FMODAsset _cinematicKillSound = AudioUtils.GetFmodAsset("TwisteelCinematicAttack");
    private PlayerCinematicController _playerDeathCinematic;
    private float _timeCanPlayCinematicAgain;

    void Start()
    {
        _playerDeathCinematic = gameObject.AddComponent<PlayerCinematicController>();
        _playerDeathCinematic.animatedTransform = playerCameraAnimatedTransform;
        _playerDeathCinematic.animator = creature.GetAnimator();
        _playerDeathCinematic.animParamReceivers = new GameObject[0];
    }

    public override void OnTouch(Collider collider)
    {
        if (frozen)
        {
            return;
        }

        if (liveMixin.IsAlive() && Time.time > timeLastBite + biteInterval)
        {
            var component = GetComponent<Creature>();
            if (component.Aggression.Value >= 0.1f)
            {
                GameObject target = GetTarget(collider);
                if (!_playerDeathCinematic.IsCinematicModeActive())
                {
                    Player player = target.GetComponent<Player>();
                    if (player != null)
                    {
                        if (Time.time > _timeCanPlayCinematicAgain && player.CanBeAttacked() &&
                            player.liveMixin.IsAlive() && !player.cinematicModeActive)
                        {
                            float num = DamageSystem.CalculateDamage(biteDamage, DamageType.Normal, player.gameObject,
                                null);
                            if (player.liveMixin.health - num <= 0f)
                            {
                                Invoke(nameof(KillPlayer), 2.5f);
                                _playerDeathCinematic.StartCinematicMode(player);
                                attackEmitter.SetAsset(_cinematicKillSound);
                                attackEmitter.Play();
                                _timeCanPlayCinematicAgain = Time.time + 5f;
                                timeLastBite = Time.time;
                                return;
                            }
                        }
                    }

                    LiveMixin liveMixin = target.GetComponent<LiveMixin>();
                    if (liveMixin == null) return;
                    if (!liveMixin.IsAlive())
                    {
                        return;
                    }

                    if (liveMixin == Player.main.liveMixin)
                    {
                        if (!player.CanBeAttacked())
                        {
                            return;
                        }
                    }

                    if (!CanAttackTargetFromPosition(target))
                    {
                        return;
                    }

                    liveMixin.TakeDamage(GetBiteDamage(target));
                    attackEmitter.SetAsset(_biteSound);
                    attackEmitter.Play();
                    timeLastBite = Time.time;
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

    private void KillPlayer()
    {
        Player.main.liveMixin.Kill(DamageType.Normal);
        _playerDeathCinematic.OnPlayerCinematicModeEnd();
    }
}