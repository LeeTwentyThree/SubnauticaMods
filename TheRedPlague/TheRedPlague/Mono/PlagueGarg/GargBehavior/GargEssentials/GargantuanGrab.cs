using System.Collections;
using System.Collections.Generic;
using Nautilus.Utility;
using TheRedPlague.PrefabFiles.GargTeaser;

namespace TheRedPlague.Mono.PlagueGarg.GargBehavior.GargEssentials;

using ECCLibrary;
using UnityEngine;

class GargantuanGrab : MonoBehaviour
{
    public float vehicleDamagePerSecond;
    public float cyclopsDamagePerSecond;
    public GargGrabFishMode grabFishMode;
    public string attachBoneName;
    public float grabAttackDamage;

    public Vehicle HeldVehicle { get; private set; }

    private Animator _gargAnimator;
    private Creature _creature;
    private Collider[] _subrootStoredColliders;
    private GargantuanBehaviour _behaviour;
    private GargantuanMouthAttack _mouthAttack;
    private SubRoot _heldSubroot;
    private GameObject _heldFish;
    private Quaternion _vehicleInitialRotation;
    private Vector3 _vehicleInitialPosition;
    private GrabType _currentlyGrabbing;
    private Transform _vehicleHoldPoint;
    private float _timeVehicleGrabbed;
    private float _timeVehicleReleased;
    private float _currentGrabRandom;
    private bool _killStandingPlayerInSub;
    private Transform _lightingRootTransform;

    public FMOD_CustomEmitter emitter;

    private static readonly FMODAsset _seamothSounds = AudioUtils.GetFmodAsset("RPGargGrabSeamoth");
    private static readonly FMODAsset _exosuitSounds = AudioUtils.GetFmodAsset("RPGargGrabExosuit");
    private static readonly FMODAsset _cyclopsSounds = AudioUtils.GetFmodAsset("RPGargGrabCyclops");
    private static readonly FMODAsset _ghostSounds = AudioUtils.GetFmodAsset("RPGargGrabGhost");
    private static readonly FMODAsset _reaperSounds = AudioUtils.GetFmodAsset("RPGargGrabReaper");

    private float _powerDrainedPerSecondVehicle = 20f;
    private float _powerDrainedPerSecondSub = 60f;

    void Start()
    {
        _creature = GetComponent<Creature>();
        _behaviour = GetComponent<GargantuanBehaviour>();
        _vehicleHoldPoint = gameObject.SearchChild(attachBoneName).transform;
        _mouthAttack = GetComponent<GargantuanMouthAttack>();
        _gargAnimator = _creature.GetAnimator();
        _lightingRootTransform = _creature.GetAnimator().transform.Find("Armature/Head");
    }

    void Update()
    {
        SafeAnimator.SetBool(_gargAnimator, "cin_vehicle", IsHoldingGenericSub() || IsHoldingExosuit());
        SafeAnimator.SetBool(_gargAnimator, "cin_cyclops", IsHoldingLargeSub());
        bool ghostAnim = false;
        bool deathRollAnim = false;
        if (IsHoldingFish())
        {
            if (_currentGrabRandom < 0.5f)
            {
                ghostAnim = true;
            }
            else
            {
                deathRollAnim = true;
            }
        }
        SafeAnimator.SetBool(_gargAnimator, "cin_ghostleviathanattack", ghostAnim);
        SafeAnimator.SetBool(_gargAnimator, "cin_deathroll", deathRollAnim);

        if (CurrentHeldObject == null)
            return;

        if (_heldSubroot != null && _heldSubroot.rb != null)
        {
            _heldSubroot.rb.isKinematic = true;
        }

        if (_killStandingPlayerInSub)
        {
            if (!Player.main.cinematicModeActive)
            {
                Player.main.liveMixin.TakeDamage(1000f);
            }
        }

        if (IsHoldingGenericSub() || IsHoldingExosuit())
        {
            CurrentHeldObject.GetComponent<Vehicle>().ConsumeEnergy(Time.deltaTime * _powerDrainedPerSecondVehicle);
        }
        else if (IsHoldingLargeSub())
        {
            CurrentHeldObject.GetComponent<SubRoot>().powerRelay.ConsumeEnergy(Time.deltaTime * _powerDrainedPerSecondSub, out float _);
        }

        Transform held = CurrentHeldObject.transform;
        Transform holdPoint = GetHoldPoint();
        float num = Mathf.Clamp01(Time.time - _timeVehicleGrabbed);
        if (num >= 1f)
        {
            if (IsGargJuvenile() && IsHoldingFish())
            {
                held.transform.position = _behaviour.FixJuvenileFishHoldPosition(holdPoint, holdPoint.position);
            }
            else if (IsHoldingLargeSub())
            {
                held.transform.position = holdPoint.position + (holdPoint.forward * -25f);
            }
            else
            {
                held.transform.position = holdPoint.position;
            }

            if (IsHoldingLargeSub())
            {
                held.transform.forward = -holdPoint.transform.forward; // cyclops faces backwards for whatever reason so we need to invert the rotation
            }
            else if (IsHoldingPickupableFish())
            {
                held.transform.rotation =
                    _behaviour.FixSmallFishRotation(holdPoint.transform.rotation); // cyclops faces backwards for whatever reason so we need to invert the rotation
            }
            else
            {
                held.transform.rotation = holdPoint.transform.rotation;
            }

            // blood vfx
            if (Time.time > _behaviour.timeSpawnBloodAgain && _behaviour.CachedBloodPrefab != null)
            {
                if (IsHoldingFish())
                {
                    _behaviour.timeSpawnBloodAgain = Time.time + 0.5f;
                    GameObject blood = Instantiate(_behaviour.CachedBloodPrefab, held.transform.position,
                        Quaternion.identity);
                    blood.SetActive(true);
                    Destroy(blood, _behaviour.bloodDestroyTime);
                }
            }

            return;
        }

        if (IsGargJuvenile() && IsHoldingFish())
        {
            held.transform.position = (_behaviour.FixJuvenileFishHoldPosition(holdPoint, holdPoint.position) - _vehicleInitialPosition) * num + _vehicleInitialPosition;
        }
        else if (IsHoldingLargeSub())
        {
            held.transform.position = ((holdPoint.position + (holdPoint.forward * -25f)) - _vehicleInitialPosition) * num + _vehicleInitialPosition;
        }
        else
        {
            held.transform.position = (holdPoint.position - this._vehicleInitialPosition) * num + _vehicleInitialPosition;
        }

        if (IsHoldingLargeSub())
        {
            held.transform.forward = -holdPoint.transform.forward; // cyclops faces backwards for whatever reason so we need to invert the rotation
        }
        else if (IsHoldingPickupableFish())
        {
            held.transform.rotation = Quaternion.Lerp(this._vehicleInitialRotation,
                _behaviour.FixSmallFishRotation(holdPoint.rotation), num); // cyclops faces backwards for whatever reason so we need to invert the rotation
        }
        else
        {
            held.transform.rotation = Quaternion.Lerp(this._vehicleInitialRotation, holdPoint.rotation, num);
        }
    }


    void OnDisable()
    {
        if (HeldVehicle != null)
        {
            ReleaseHeld();
        }
    }


    GameObject CurrentHeldObject =>
        HeldVehicle ? HeldVehicle.gameObject :
        _heldSubroot ? _heldSubroot.gameObject :
        _heldFish ? _heldFish : null;

    Transform GetHoldPoint()
    {
        return _vehicleHoldPoint;
    }

    public bool IsHoldingAnything()
    {
        return _currentlyGrabbing != GrabType.None;
    }

    /// <summary>
    /// Holding Seamoth or Seatruck.
    /// </summary>
    /// <returns></returns>
    public bool IsHoldingGenericSub()
    {
        return _currentlyGrabbing == GrabType.GenericVehicle;
    }

    /// <summary>
    /// Holding a Cyclops.
    /// </summary>
    /// <returns></returns>
    public bool IsHoldingLargeSub()
    {
        return _currentlyGrabbing == GrabType.Cyclops;
    }

    public bool IsHoldingPickupableFish()
    {
        return _currentlyGrabbing == GrabType.Fish && grabFishMode == GargGrabFishMode.PickupableOnlyAndSwalllow;
    }

    public bool IsGargJuvenile()
    {
        return grabFishMode == GargGrabFishMode.LeviathansOnlyNoSwallow;
    }

    public bool IsHoldingExosuit()
    {
        return _currentlyGrabbing == GrabType.Exosuit;
    }

    public bool IsHoldingFish()
    {
        return _currentlyGrabbing == GrabType.Fish;
    }

    public bool IsHoldingGhostLeviathan()
    {
        if (_currentlyGrabbing != GrabType.Fish)
        {
            return false;
        }

        if (_heldFish == null) 
            return false;
            
        if (_heldFish.GetComponent<GhostLeviathan>() is not null || _heldFish.GetComponent<GhostLeviatanVoid>() is not null)
        {
            return true;
        }

        return false;
    }

    public bool IsHoldingReaperLeviathan()
    {
        if (_currentlyGrabbing != GrabType.Fish)
        {
            return false;
        }

        if (_heldFish != null)
        {
            if (_heldFish.GetComponent<ReaperLeviathan>() is not null)
            {
                return true;
            }
        }

        return false;
    }

    enum GrabType
    {
        None,
        Exosuit,
        GenericVehicle,
        Cyclops,
        Fish
    }

    public void GrabLargeSub(SubRoot subRoot)
    {
        GrabSubRoot(subRoot);
    }

    public void GrabGenericSub(Vehicle vehicle)
    {
        GrabVehicle(vehicle, GrabType.GenericVehicle);
    }

    public void GrabExosuit(Vehicle exosuit)
    {
        GrabVehicle(exosuit, GrabType.Exosuit);
    }

    public bool GetCanGrabVehicle()
    {
        if (grabFishMode == GargGrabFishMode.LeviathansOnlyAndSwallow)
        {
            return !IsHoldingAnything();
        }
        return _timeVehicleReleased + 4f < Time.time && !IsHoldingAnything();
    }

    #region Initial grab logic
    private void GrabSubRoot(SubRoot subRoot)
    {
        var playerInSub = subRoot == Player.main.GetCurrentSub();
        _currentGrabRandom = Random.value;
        _heldSubroot = subRoot;
        _currentlyGrabbing = GrabType.Cyclops;
        _timeVehicleGrabbed = Time.time;
        var subRootTransform = subRoot.transform;
        _vehicleInitialRotation = subRootTransform.rotation;
        _vehicleInitialPosition = subRootTransform.position;
        PlaySound(_cyclopsSounds);
        FreezeRigidbodyWhenFar freezeRb = subRoot.GetComponent<FreezeRigidbodyWhenFar>();
        if (freezeRb)
        {
            freezeRb.enabled = false;
        }
        Stabilizer stabilizer = subRoot.GetComponent<Stabilizer>();
        if (stabilizer)
        {
            stabilizer.enabled = false;
        }

        _subrootStoredColliders = subRoot.GetComponentsInChildren<Collider>(false);
        for (int i = 0; i < _subrootStoredColliders.Length; i++)
        {
            if (_subrootStoredColliders[i].GetComponentInParent<Player>() != null)
            {
                _subrootStoredColliders[i] = null;
            }
        }
        ToggleSubrootColliders(false);
        subRoot.rigidbody.isKinematic = true;
        InvokeRepeating(nameof(DamageVehicle), 1f, 1f);
        float attackLength = 11f;
        Invoke(nameof(ReleaseHeld), attackLength);
        MainCameraControl.main.ShakeCamera(5f, attackLength, MainCameraControl.ShakeMode.BuildUp, 1.2f);
        _behaviour.timeCanAttackAgain = Time.time + attackLength + 1f;

        var creatureAttackVoice = subRoot.creatureAttackNotification;
        if (creatureAttackVoice != null)
        {
            creatureAttackVoice.timeNextPlay = 0f; //force the line to play
            creatureAttackVoice.Play();
        }
        if (playerInSub)
        {
            _killStandingPlayerInSub = true;
        }
    }

    private void GrabVehicle(Vehicle vehicle, GrabType vehicleType)
    {
        _currentGrabRandom = Random.value;
        vehicle.useRigidbody.isKinematic = true;
        vehicle.collisionModel.SetActive(false);
        HeldVehicle = vehicle;
        _currentlyGrabbing = vehicleType;
        if (_currentlyGrabbing == GrabType.Exosuit)
        {
            SafeAnimator.SetBool(vehicle.mainAnimator, "reaper_attack", true);
            Exosuit component = vehicle.GetComponent<Exosuit>();
            if (component != null)
            {
                component.cinematicMode = true;
            }
        }

        _timeVehicleGrabbed = Time.time;
        var vehicleTransform = vehicle.transform;
        _vehicleInitialRotation = vehicleTransform.rotation;
        _vehicleInitialPosition = vehicleTransform.position;
        if (_currentlyGrabbing == GrabType.GenericVehicle)
        {
            PlaySound(_seamothSounds);
        }
        else if (_currentlyGrabbing == GrabType.Exosuit)
        {
            PlaySound(_exosuitSounds);
        }
        else
        {
            ErrorMessage.AddMessage("Unknown Vehicle Type detected");
        }

        foreach (Collider col in vehicle.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }

        InvokeRepeating(nameof(DamageVehicle), 1f, 1f);
        float attackLength = 4f;
        Invoke(nameof(ReleaseHeld), attackLength);
        if (Player.main.GetVehicle() == HeldVehicle)
        {
            MainCameraControl.main.ShakeCamera(4f, attackLength, MainCameraControl.ShakeMode.BuildUp, 1.2f);
        }
    }

    public void GrabFish(GameObject fish)
    {
        _currentGrabRandom = Random.value;
        _heldFish = fish;
        _currentlyGrabbing = GrabType.Fish;
        _timeVehicleGrabbed = Time.time;

        _vehicleInitialRotation = fish.transform.rotation;
        _vehicleInitialPosition = fish.transform.position;

        if (IsHoldingGhostLeviathan())
        {
            PlaySound(_ghostSounds);
            // PlayLightningFX(6f);
        }
        else if (IsHoldingReaperLeviathan())
        {
            PlaySound(_reaperSounds);
        }

        SetPreyGrabbed(fish, true);

        if (grabFishMode == GargGrabFishMode.LeviathansOnlyAndSwallow)
        {
            _behaviour.GetBloodEffectFromCreature(fish, 40f, 2f);
            _behaviour.timeSpawnBloodAgain = Time.time + 1f;
        }

        if (grabFishMode == GargGrabFishMode.LeviathansOnlyNoSwallow)
        {
            _behaviour.GetBloodEffectFromCreature(fish, 20f, 2f);
            _behaviour.timeSpawnBloodAgain = Time.time + 1f;
        }

        if (grabFishMode == GargGrabFishMode.PickupableOnlyAndSwalllow)
        {
            _behaviour.GetBloodEffectFromCreature(fish, 0.5f, 1f);
            _behaviour.timeSpawnBloodAgain = Time.time + 1f;
        }

        Invoke(nameof(ReleaseHeld), 5f);
    }
    #endregion

    private void SetPreyGrabbed(GameObject prey, bool grabbed)
    {
        if (prey == null) return;
        prey.GetComponent<Rigidbody>().isKinematic = !grabbed;
        foreach (var col in prey.GetComponentsInChildren<Collider>(true))
        {
            if (!col.isTrigger)
            {
                col.enabled = !grabbed;
            }
        }
    }

    /// <summary>
    /// Try to deal damage to the held vehicle or subroot
    /// </summary>
    void DamageVehicle()
    {
        if (HeldVehicle != null)
        {
            float dps = vehicleDamagePerSecond;

            if (Player.main.currentMountedVehicle == HeldVehicle)
            {
                float calcDamage = DamageSystem.CalculateDamage(dps, DamageType.Normal, HeldVehicle.gameObject);
                if (HeldVehicle.liveMixin.health - calcDamage < 0f)
                {

                    Player.main.liveMixin.Kill(DamageType.Normal);
                }
            }

            HeldVehicle.liveMixin.TakeDamage(dps, type: DamageType.Normal, dealer: gameObject);
        }

        if (_heldSubroot != null)
        {
            _heldSubroot.live.TakeDamage(cyclopsDamagePerSecond, type: DamageType.Normal);
        }
    }

    /// <summary>
    /// Try to release the held vehicle or subroot
    /// </summary>
    public void ReleaseHeld()
    {
        if (HeldVehicle != null)
        {
            if (_currentlyGrabbing == GrabType.Exosuit)
            {
                SafeAnimator.SetBool(HeldVehicle.mainAnimator, "reaper_attack", false);
                Exosuit component = HeldVehicle.GetComponent<Exosuit>();
                if (component != null)
                {
                    component.cinematicMode = false;
                }
            }
            
            HeldVehicle.useRigidbody.isKinematic = false;
            HeldVehicle.useRigidbody.velocity = transform.forward * (IsGargJuvenile() ? 10 : 50);
            HeldVehicle.collisionModel.SetActive(true);
            HeldVehicle = null;
        }

        if (_heldSubroot != null)
        {
            FreezeRigidbodyWhenFar freezeRb = _heldSubroot.GetComponent<FreezeRigidbodyWhenFar>();
            if (freezeRb)
            {
                freezeRb.enabled = true;
            }
            Stabilizer stabilizer = _heldSubroot.GetComponent<Stabilizer>();
            if (stabilizer)
            {
                stabilizer.enabled = true;
            }

            _heldSubroot.rigidbody.isKinematic = false;
            _heldSubroot.rigidbody.velocity = transform.forward * (IsGargJuvenile() ? 10 : 50);
            ToggleSubrootColliders(true);
            _heldSubroot = null;
        }

        if (_heldFish != null)
        {
            var creatureLm = _heldFish.GetComponent<LiveMixin>();
            if (grabFishMode == GargGrabFishMode.LeviathansOnlyAndSwallow ||
                grabFishMode == GargGrabFishMode.PickupableOnlyAndSwalllow)
            {
                float animationLength =
                    (grabFishMode == GargGrabFishMode.PickupableOnlyAndSwalllow) ? 0.25f : 0.75f;
                var swallowing = _heldFish.AddComponent<DevouredLiveAnimation>();
                swallowing.throatTarget = _mouthAttack.throat.transform;
                swallowing.animationLength = animationLength;
                Destroy(_heldFish, animationLength);
            }
            else
            {
                creatureLm.TakeDamage(grabAttackDamage);
                SetPreyGrabbed(_heldFish, true);

                _heldFish = null;
            }
        }

        if (_currentlyGrabbing != GrabType.Fish)
        {
            _timeVehicleReleased = Time.time;
        }

        Invoke(nameof(AllowPlayerToWalkInSub), 3f);
        _currentlyGrabbing = GrabType.None;
        CancelInvoke(nameof(DamageVehicle));
        _mouthAttack.OnVehicleReleased();
        MainCameraControl.main.ShakeCamera(0f, 0f);
        if (_behaviour.lastTarget) _behaviour.lastTarget.target = null;
    }

    void AllowPlayerToWalkInSub()
    {
        _killStandingPlayerInSub = false;
    }

    /// <summary>
    /// Disable cyclops colliders during garg cyclops attack animation
    /// </summary>
    /// <param name="active"></param>
    void ToggleSubrootColliders(bool active)
    {
        if (_subrootStoredColliders != null)
        {
            foreach (Collider col in _subrootStoredColliders)
            {
                if (col != null)
                {
                    col.enabled = active;
                }
            }
        }
    }

    void PlaySound(FMODAsset asset)
    {
        emitter.SetAsset(asset);
        emitter.Play();
    }
}