using System;
using Nautilus.Utility;
using TheRedPlague.PrefabFiles.GargTeaser;

namespace TheRedPlague.Mono.PlagueGarg.GargBehavior.GargEssentials;

using ECCLibrary;
using System.Collections;
using UnityEngine;
using static GargantuanConditions;

internal class GargantuanMouthAttack : MeleeAttack
{
    private GargantuanBehaviour _behaviour;
    private GargantuanGrab _grab;
    private Creature _garg;
    private PlayerCinematicController _playerDeathCinematic;
    private Rigidbody _rb;
    private float _aggressionThreshold = 0.2f;
    private float _nibbleHungerThreshold = 0.2f;
        
    private static readonly int _gargBiteAnimParam = Animator.StringToHash("bite");
    private static readonly int _gargRandomAnimParam = Animator.StringToHash("random");

    public GameObject throat;
    public bool canAttackPlayer = true;
    public bool instantlyKillsPlayer;
    public string attachBoneName;
    public bool canPerformCyclopsCinematic;
    public GargGrabFishMode grabFishMode;
    public FMOD_CustomEmitter emitter;

    private static readonly FMODAsset _biteSound = AudioUtils.GetFmodAsset("RPGargBite");
    private static readonly FMODAsset _cinematicKillSound = AudioUtils.GetFmodAsset("RPGargEatPlayerCinematic");

    private void Start()
    {
        _garg = GetComponent<Creature>();
        _grab = GetComponent<GargantuanGrab>();
        _rb = GetComponent<Rigidbody>();
        throat = gameObject.SearchChild("Head");
        var onTouch = gameObject.SearchChild("Mouth").EnsureComponent<OnTouch>().onTouch = new OnTouch.OnTouchEvent();
        onTouch.AddListener(OnTouch);
        _behaviour = GetComponent<GargantuanBehaviour>();

        _playerDeathCinematic = gameObject.AddComponent<PlayerCinematicController>();
        _playerDeathCinematic.animatedTransform = gameObject.SearchChild(attachBoneName).transform;
        _playerDeathCinematic.animator = creature.GetAnimator();
        _playerDeathCinematic.animParamReceivers = Array.Empty<GameObject>();
        _playerDeathCinematic.animParam = "cin_player";
        _playerDeathCinematic.playerViewAnimationName = "seadragon_attack";
    }

    public override void OnTouch(Collider collider)
    {
        // should never happen, cuz the garg is unable to die but just in case.
        if (!liveMixin.IsAlive() || !_behaviour.CanPerformAttack() || _playerDeathCinematic.IsCinematicModeActive())
            return;

        var target = GetTarget(collider);
        if (!_behaviour.CanEat(target)) // check if we can do anything with the target, otherwise early exit.
            return;
            
        if (_grab.IsHoldingAnything()) // shouldn't perform another attack if already attacking something.
            return;

        var targetLm = target.GetComponent<LiveMixin>();
        var player = target.GetComponent<Player>();
        if (player) // player-based attacks
        {
            if (!PlayerAttackable(player))
                return;
                
            // if we cant attack the player, then we're the baby and should nibble his hand.
            if (!canAttackPlayer)
            {
                if (_garg.Hunger.Value > _nibbleHungerThreshold)
                {
                    GargantuanBabyNibble();
                    _behaviour.timeCanAttackAgain = Time.time + 1f;
                }
                return;
            }

            // otherwise screw him up like a truck.
                
            var damage = instantlyKillsPlayer ? 1000f : 80f;

            var num = DamageSystem.CalculateDamage(damage, DamageType.Normal, target);
            if (targetLm.health - num <= 0f) // check if the player's gonna die during this attack
            {
                StartCoroutine(PerformPlayerCinematic(player));
                return;
            }
                
            // if not, then just bite him.
            StartCoroutine(PerformBiteAttack(target, damage));
            _behaviour.timeCanAttackAgain = Time.time + 2f;
            return;
        }

        var targetSubRoot = target.GetComponent<SubRoot>(); // reference of the sub root for later use

        if (canAttackPlayer && _grab.GetCanGrabVehicle()) // vehicles-based attacks
        {
            // look for smaller vehicles (i.e. seamoth or exosuit)
            var vehicle = target.GetComponent<Vehicle>();

            if (vehicle)
            {
                if (vehicle is Exosuit && !vehicle.docked) // if it's an exosuit, we grab it like an exosuit
                    _grab.GrabExosuit(vehicle);
                else if (!vehicle.docked) // if it's whatever else type of vehicles, we grab it like a generic one.
                    _grab.GrabGenericSub(vehicle);

                _garg.Aggression.Value -= 0.5f;
                return;
            }

            if (canPerformCyclopsCinematic)
            {
                if (targetSubRoot && !targetSubRoot.rb.isKinematic && targetSubRoot.live) // checks for cyclops
                {
                    _grab.GrabLargeSub(targetSubRoot);
                    _behaviour.roar.DelayTimeOfNextRoar(8f);
                    _garg.Aggression.Value -= 1f;
                    return;
                }
            }
        }
        if (!targetLm)
            return;
                
        if (!targetLm.IsAlive()) // shouldn't attack dead meat
            return;

        var targetCreature = target.GetComponent<Creature>();
        
        if (!targetCreature && (!canPerformCyclopsCinematic && targetSubRoot == null)) // only bite creatures and things that are marked as edible (and subroots if you're a juvenile)
            return;

        // only can perform a grab attack on creatures
        var isCreature = targetCreature != null;
        if (isCreature)
        {
            switch (grabFishMode)
            {
                // Leviathan attack animations
                case GargGrabFishMode.LeviathansOnlyAndSwallow or GargGrabFishMode.LeviathansOnlyNoSwallow:
                {
                    if ((grabFishMode == GargGrabFishMode.LeviathansOnlyAndSwallow && AdultCanGrab(target))
                        ||
                        (grabFishMode == GargGrabFishMode.LeviathansOnlyNoSwallow && JuvenileCanGrab(target)))
                    {
                        GargantuanCreatureAttack(ref targetCreature, false);
                        return;
                    }
                    break;
                }
                // baby "play with food" animation
                case GargGrabFishMode.PickupableOnlyAndSwalllow:
                {
                    if (target.GetComponent<Pickupable>() != null && target.GetComponent<GargantuanRoar>() == null)
                    {
                        GargantuanCreatureAttack(ref targetCreature, true);
                        return;
                    }
                    break;
                }
            }
        }
            
        if (!CanAttackTargetFromPosition(target)) // any attack past this point must not have collisions between the garg and the target
            return;
            
        if (isCreature && CanSwallowWhole(target, targetLm)) // if the garg should perform a "slurp" attack that destroys the creature quickly and in an interesting manner
        {
            creature.GetAnimator().SetTrigger("bite");
            _garg.Hunger.Value -= 0.5f;
            var swallowing = target.AddComponent<DevouredLiveAnimation>();
            swallowing.throatTarget = throat.transform;
            swallowing.animationLength = 1f;
            return;
        }

        // if all of that fails, just try to do a bite attack on whatever this object is

        if (!canAttackPlayer && IsVehicleOrSub(target)) // the baby garg shouldn't bite vehicles or subs
        {
            return;
        }

        StartCoroutine(PerformBiteAttack(target, GetBiteDamage(target)));

        bool bitingCyclops = target.GetComponentInParent<SubRoot>();

        _behaviour.timeCanAttackAgain = Time.time + (bitingCyclops ? 15f : 2f); // set a delay on when you can attack again
    }

    // Called when frozen by the stasis rifle
    private new void OnFreezeByStasisSphere()
    {
        base.OnFreezeByStasisSphere();
        _rb.isKinematic = false;
    }

    private void GargantuanCreatureAttack(ref Creature targetCreature, bool isGargBaby)
    {
        _garg.Aggression.Value -= 0.6f;
        _garg.Hunger.Value = 0f;
        _garg.Happy.Value += 1f;
        targetCreature.Scared.Value = 1f;
        _grab.GrabFish(targetCreature.gameObject);
        if (isGargBaby)
            targetCreature.liveMixin.TakeDamage(1f, targetCreature.transform.position);
        Destroy(targetCreature.GetComponent<EcoTarget>());
    }

    // Looks for what's the player is holding and eats it if it's a creature.
    private void GargantuanBabyNibble()
    {
        var held = Inventory.main.GetHeld();
        if (held is not null && held.GetComponent<Creature>() != null)
        {
            var heldLm = held.GetComponent<LiveMixin>();
            if (heldLm.maxHealth < 100f)
            {
                animator.SetFloat("random", Random.value);
                animator.SetTrigger("bite");
                emitter.SetAsset(_biteSound);
                emitter.Play();
                _garg.Hunger.Value -= 0.2f;
                _garg.Happy.Value += 1f;
                if (_behaviour.GetBloodEffectFromCreature(held.gameObject, 1f, 1f))
                {
                    var blood = Instantiate(_behaviour.CachedBloodPrefab, held.transform.position, Quaternion.identity);
                    blood.SetActive(true);
                    Destroy(blood, _behaviour.bloodDestroyTime);
                }
                Destroy(held.gameObject);
            }
        }
    }

    private bool PlayerAttackable(Player player)
    {
        if (player.GetCurrentSub() != null || player.GetVehicle() != null)
        {
            return false;
        }
        return player.CanBeAttacked() && (player.liveMixin.IsAlive() || !player.cinematicModeActive || PlayerIsKillable());
    }

    private bool CanAttackTargetFromPosition(GameObject target) // A quick raycast check to stop the Gargantuan from attacking through walls. Taken from the game's code (shh).
    {
        var direction = target.transform.position - transform.position;
        var magnitude = direction.magnitude;
        var num = UWE.Utils.RaycastIntoSharedBuffer(transform.position, direction, magnitude, -5, QueryTriggerInteraction.Ignore);
        for (int i = 0; i < num; i++)
        {
            var collider = UWE.Utils.sharedHitBuffer[i].collider;
            var attachedRigidbody = collider.attachedRigidbody;
            // ReSharper disable once Unity.NoNullCoalescing
            var hitGameObject = attachedRigidbody != null ? attachedRigidbody.gameObject : collider.gameObject;
            if (hitGameObject != target && hitGameObject != gameObject && hitGameObject.GetComponent<Creature>() == null)
            {
                return false;
            }
        }

        return true;
    }

    public override float GetBiteDamage(GameObject target) // Extra damage to Cyclops. Otherwise, does its base damage.
    {
        if (target.GetComponent<SubControl>() != null)
        {
            return 500f; // cyclops damage
        }

        return biteDamage; // base damage
    }

    public void OnVehicleReleased() // Called by gargantuan behavior. Gives a cooldown until the next bite.
    {
        _behaviour.timeCanAttackAgain = Time.time + 4f;
    }

    private IEnumerator PerformBiteAttack(GameObject target, float damage) // A delayed attack, to let him chomp down first.
    {
        animator.SetFloat(_gargRandomAnimParam, Random.value);
        animator.SetTrigger(_gargBiteAnimParam);
        emitter.SetAsset(_biteSound);
        emitter.Play();
        yield return new WaitForSeconds(0.5f);
        if (target)
        {
            var targetLm = target.GetComponent<LiveMixin>();
            if (targetLm)
            {
                targetLm.TakeDamage(damage, transform.position, DamageType.Normal, gameObject);
                if (!targetLm.IsAlive())
                {
                    creature.Aggression.Value = 0f;
                    creature.Hunger.Value = 0f;
                    creature.Happy.Value += 1f;
                }
            }
        }
    }

    private IEnumerator PerformPlayerCinematic(Player player)
    {
        if (instantlyKillsPlayer)
        {
            // CustomPDALinesManager.PlayVoiceLine("PDADeathImminent");
        }

        _playerDeathCinematic.enabled = true;
        _playerDeathCinematic.StartCinematicMode(player);
        float length = 1.8f;
        emitter.SetAsset(_cinematicKillSound);
        emitter.Play();
        _behaviour.timeCanAttackAgain = Time.time + length;
        yield return new WaitForSeconds(length / 3f);
        var position = transform.position;
        Player.main.liveMixin.TakeDamage(5f, position, DamageType.Normal, gameObject);
        yield return new WaitForSeconds(length / 3f);
        Player.main.liveMixin.TakeDamage(5f, position, DamageType.Normal, gameObject);
        yield return new WaitForSeconds(length / 3f);
        _playerDeathCinematic.enabled = false;
        Player.main.liveMixin.TakeDamage(250f, position, DamageType.Normal, gameObject);
    }
}