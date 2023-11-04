using UnityEngine;
using System.Collections;

namespace TheRumbling.Mono;

internal class WallTitan : MonoBehaviour
{
    public Transform heatEmitter;
    public HeightmapEntity heightmapEntity;

    private Animator animator;
    private static readonly int _walkSpeedParam = Animator.StringToHash("walk_speed");
    private static readonly int _walkParam = Animator.StringToHash("walk");
    private static readonly int _standingParam = Animator.StringToHash("standing");

    private GameObject _normalModel;
    private GameObject _staticModel;
    private GameObject _ragdollModel;
    
    private float _yVelocity;

    private bool _emoting;

    private static Emote[] emotes = new Emote[]
    {
        new Emote("dying", 16),
        new Emote("fallflat", 3),
        new Emote("idlelooking", 8),
        new Emote("joyfuljump", 2),
        new Emote("mmakick", 2),
        new Emote("standingbattlecry", 3),
        new Emote("twistdance", 10),
        new Emote("dancingtwerk", 16),
    };

    private struct Emote
    {
        public string Name { get; private set; }
        public float Duration { get; private set; }

        public Emote(string name, float duration)
        {
            Name = name;
            Duration = duration;
        }
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat(_walkSpeedParam, 1f);
        transform.localScale = Vector3.one * Balance.DefaultTitanScale;

        _normalModel = transform.Find("Main Titan Model").gameObject;
        _staticModel = transform.Find("Static Titan Model").gameObject;
        _ragdollModel = transform.Find("Ragdoll Model").gameObject;

        foreach (var rb in _ragdollModel.GetComponentsInChildren<Rigidbody>(true))
        {
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.useGravity = false;
            rb.gameObject.AddComponent<WorldForces>().useRigidbody = rb;
        }
        
        InvokeRepeating(nameof(ScanForAurora), Random.value, 2f);
    }

    private void Update()
    {
        if (_emoting)
        {
            animator.SetBool(_walkParam, false);
            animator.SetBool(_standingParam, true);
            return;
        }
        animator.SetBool(_walkParam, !heightmapEntity.Swimming);
        animator.SetBool(_standingParam, false);
    }

    private void ScanForAurora()
    {
        if (Physics.Raycast(transform.position, transform.forward, out var hit, 60, -1))
        {
            if (hit.collider.gameObject.GetComponentInParent<CrashedShipExploder>() != null)
            {
                StartCoroutine(KickAurora(CrashedShipExploder.main.gameObject));
            }
        }
    }

    private IEnumerator KickAurora(GameObject aurora)
    {
        PlayStaticAnimation("mmakick", 2);
        yield return new WaitForSeconds(1.2f);
        var rb = aurora.EnsureComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.mass = 6000;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.AddForce(transform.forward * 100, ForceMode.VelocityChange);
        rb.AddTorque(new Vector3(Random.value * 360f, Random.value * 360f, Random.value * 360f) * 100, ForceMode.VelocityChange);
    }

    private void OnEnable() => RumblingManager.RegisterTitan(this);

    private void OnDisable() => RumblingManager.UnregisterTitan(this);

    private void SetActiveModel(GameObject model)
    {
        _normalModel.SetActive(model == _normalModel);
        _staticModel.SetActive(model == _staticModel);
        _ragdollModel.SetActive(model == _ragdollModel);
    }
    
    public void Ragdoll()
    {
        SetActiveModel(_ragdollModel);
        heightmapEntity.canMove = false;
    }

    public void PlayRandomEmote()
    {
        var random = emotes[Random.Range(0, emotes.Length)];
        PlayStaticAnimation(random.Name, random.Duration);
    }

    public void PlayStaticAnimation(string trigger, float duration)
    {
        StartCoroutine(StaticAnimCoroutine(trigger, duration));
    }

    private IEnumerator StaticAnimCoroutine(string trigger, float dur)
    {
        _emoting = true;
        heightmapEntity.canMove = false;
        yield return new WaitForSeconds(0.67f);
        SetActiveModel(_staticModel);
        _staticModel.GetComponent<Animator>().SetTrigger(trigger);
        yield return new WaitForSeconds(dur);
        _emoting = false;
        SetActiveModel(_normalModel);
        heightmapEntity.canMove = true;
    }
}