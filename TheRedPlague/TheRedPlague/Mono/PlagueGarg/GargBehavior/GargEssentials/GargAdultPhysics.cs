using System.Collections.Generic;
using UnityEngine;

namespace TheRedPlague.Mono.PlagueGarg.GargBehavior.GargEssentials;

// general class for things related to the gargantuan adult leviathan's body collisions and other interactions it may have with the world, including water currents
internal class GargAdultPhysics : MonoBehaviour
{
    public GargantuanBehaviour gargantuanBehaviour;

    private GameObject _collisionRoot;
    private List<BodyCollider> _bodyColliders;
    private AnimationCurve _thiccnessCurve = new AnimationCurve(new Keyframe(0f, 50f), new Keyframe(1f, 30f));
    private Collider[] _headColliders;

    private bool _generateCurrents = true;
    private float _timeGenerateCurrentsAgain;
    private int _currentsColliderIndex;

    private float _currentsRadius = 110;
    // surely there is a better way to do this:
    private float _currentsInterval = 0.10f;
    private int _currentsPerTick = 3;
    private float _currentForceDuration = 1f;
    private float _smallFishCurrentAccel = 80;
    // The player is a "medium fish"
    private float _mediumFishCurrentAccel = 30f;
    // Includes the Seamoth
    private float _largeFishCurrentAccel = 20f;
    // Includes the cyclops and prawn suit
    private float _leviathanCurrentAccel = 10f;

    private bool _collidersEnabled = true;

    private Vector3[] _prevBonePositions;

    private void SetupCollisions()
    {
        var spines = gargantuanBehaviour.GetSpineBoneList();

        _headColliders = gameObject.GetComponentsInChildren<Collider>();

        _collisionRoot = new GameObject(gameObject.name + "-BodyCollision");
        _collisionRoot.AddComponent<TechTag>().type = CraftData.GetTechType(gameObject);
        // _collisionRoot.transform.parent = transform;

        _bodyColliders = new List<BodyCollider>();
        int colliderIndex = 0;
        for (int i = 1; i < spines.Count; i += 2)
        {
            if (i > spines.Count)
            {
                break;
            }

            float percentDownSpine = (float) i / spines.Count;

            var colliderObject = new GameObject($"Collider-{colliderIndex}");
            colliderObject.transform.parent = _collisionRoot.transform;

            var rb = colliderObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.mass = 1E9F;

            var capsule = colliderObject.gameObject.AddComponent<CapsuleCollider>();
            capsule.radius = _thiccnessCurve.Evaluate(percentDownSpine);
            capsule.height = 200f;
            var bodyCollider = new BodyCollider(capsule.transform, capsule, spines[i], rb);
            _bodyColliders.Add(bodyCollider);
            UpdateBodyCollider(bodyCollider);
            colliderObject.AddComponent<GargantuanBone>().creature = gargantuanBehaviour.creature;
            foreach (var headCollider in _headColliders) // very important so the garg does not become a rocket ship and launch itself into the stratosphere
            {
                Physics.IgnoreCollision(headCollider, capsule);
            }
            colliderIndex += 1;
        }

        _prevBonePositions = new Vector3[_bodyColliders.Count];
        for (int i = 0; i < _bodyColliders.Count; i++)
        {
            _prevBonePositions[i] = _bodyColliders[i].bone.position;
        }
    }

    private void Start()
    {
        SetupCollisions();
    }

    private void OnEnable()
    {
        if (_collisionRoot != null)
        {
            _collisionRoot.SetActive(true);
        }
    }
    
    private void OnDisable()
    {
        if (_collisionRoot != null)
        {
            _collisionRoot.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (_bodyColliders == null)
        {
            return;
        }
        foreach (var collider in _bodyColliders)
        {
            UpdateBodyCollider(collider);
        }
    }

    private void Update()
    {
        if (_bodyColliders == null)
        {
            return;
        }
        var shouldUseCollision = ShouldUseCollisionThisFrame();
        ToggleColliders(shouldUseCollision);
        if (shouldUseCollision) UpdateCurrents();
    }

    private void ToggleColliders(bool newState)
    {
        if (_collidersEnabled == newState)
        {
            return;
        }
        _collidersEnabled = newState;
        foreach (var collider in _bodyColliders)
        {
            collider.colliderComponent.enabled = newState;
        }
    }

    private void UpdateCurrents()
    {
        if (!_generateCurrents)
        {
            return;
        }
        if (Time.time < _timeGenerateCurrentsAgain)
        {
            return;
        }
        _timeGenerateCurrentsAgain = Time.time + _currentsInterval;
        for (int i = 0; i < _currentsPerTick; i++)
        {
            _currentsColliderIndex++;
            if (_currentsColliderIndex >= _bodyColliders.Count)
            {
                _currentsColliderIndex = 0;
            }
        }
    }

    private void UpdateBodyCollider(BodyCollider collider)
    {
        collider.rigidbody.position = collider.bone.position;
        collider.rigidbody.rotation = collider.bone.rotation;
    }

    private bool ShouldUseCollisionThisFrame()
    {
        if (Player.main.precursorOutOfWater)
        {
            return false;
        }
        if (Player.main.GetPilotingChair() != null && Player.main.GetPilotingChair().subRoot.isCyclops || Player.main.GetVehicle() != null)
        {
            return true;
        }
        if (!Player.main.IsSwimming())
        {
            return false;
        }
        return true;
    }

    public class BodyCollider
    {

        public Transform colliderTransform;
        public Collider colliderComponent;
        public Transform bone;
        public Rigidbody rigidbody;

        public BodyCollider(Transform colliderTransform, Collider colliderComponent, Transform bone, Rigidbody rigidbody)
        {
            this.colliderTransform = colliderTransform;
            this.colliderComponent = colliderComponent;
            this.bone = bone;
            this.rigidbody = rigidbody;
        }
    }
}
