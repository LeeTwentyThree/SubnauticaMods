using System.Collections;
using UnityEngine;

namespace TheRedPlague.Mono;

public class PrawnSuitCinematic : MonoBehaviour
{
    private Vector3 _startPos = new Vector3(416, 23, 1173);
    private Vector3 _walkTowardsPos = new Vector3(399, 19.5f, 1107);
    private Vector3 _jumpOffPos = new Vector3(412, 18, 1104);

    private Animator _animator;
    private Rigidbody _rb;
    private bool _walkingForward;
    private bool _jumping;
    private float _walkSpeed;

    private Quaternion _targetRotation;

    private float _animWalkSpeed;

    public static void PlayCinematic()
    {
        UWE.CoroutineHost.StartCoroutine(Cinematic());
    }

    private static IEnumerator Cinematic()
    {
        // copy paste lol

        var task = CraftData.GetPrefabForTechTypeAsync(TechType.Exosuit);
        yield return task;
        var exo = task.GetResult();
        var arm = exo.GetComponent<Exosuit>().armPrefabs[0].prefab;
        var instance = Instantiate(exo.transform.Find("exosuit_01").gameObject);
        instance.name = "PrawnSuitCinematic";
        instance.SetActive(false);
        var collider = instance.AddComponent<CapsuleCollider>();
        collider.center = Vector3.zero;
        collider.radius = 2;
        collider.height = 5;
        collider.center = Vector3.down;
        var leftArm = Instantiate(arm, instance.transform.Find("root/geoChildren/lArm_clav")).transform;
        leftArm.localPosition = Vector3.zero;
        var rightArm = Instantiate(arm, instance.transform.Find("root/geoChildren/rArm_clav"));
        rightArm.transform.localPosition = Vector3.zero;
        rightArm.transform.localScale = new Vector3(-1, 1, 1);
        var rb = instance.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.mass = 800;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        var wf = instance.AddComponent<WorldForces>();
        wf.useRigidbody = rb;
        wf.underwaterGravity = 7;
        wf.underwaterDrag = 2;
        wf.aboveWaterDrag = 1.5f;
        wf.aboveWaterGravity = 20;
        instance.SetActive(true);
        var animator = instance.GetComponent<Animator>();
        animator.SetBool("onGround", true);
        animator.SetBool("enterAnimation", true);
        animator.SetBool("player_in", true);

        instance.AddComponent<PrawnSuitCinematic>();
    }

    private IEnumerator Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        transform.position = _startPos;
        yield return WalkTowards(_walkTowardsPos, 25);
        yield return new WaitForSeconds(2f);
        yield return WalkTowards(_jumpOffPos, 25);
        yield return new WaitForSeconds(0.5f);
        yield return Jump(1.5f);
        _walkingForward = true;
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    private IEnumerator WalkTowards(Vector3 pos, float speed)
    {
        // _rb.isKinematic = true;
        _walkSpeed = speed;
        var targetDir = (new Vector3(pos.x, 0, pos.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized;
        _targetRotation = Quaternion.LookRotation(targetDir);
        while (Vector3.Dot(transform.forward, targetDir) < 0.99f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, Time.deltaTime * 180f);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        _walkingForward = true;
        while (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(pos.x, pos.z)) > 1)
        {
            yield return null;
        }

        _walkingForward = false;
    }

    private IEnumerator Jump(float seconds)
    {
        yield return PlayThrustFX();
        // _rb.isKinematic = false;
        _jumping = true;
        _animator.SetBool("reaper_attack", true);
        yield return new WaitForSeconds(seconds);
        _jumping = false;
        _animator.SetBool("reaper_attack", false);
    }

    private void Update()
    {
        _animator.SetFloat("move_speed_z", _animWalkSpeed);
        _animWalkSpeed = Mathf.MoveTowards(_animWalkSpeed, _walkingForward ? 1 : 0, Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (_walkingForward)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, Time.deltaTime * 180f);
            if (Physics.Raycast(transform.position + new Vector3(0, -3, 0), transform.forward, 4, -1, QueryTriggerInteraction.Ignore))
            {
                _rb.AddForce(Vector3.up * 40, ForceMode.Acceleration);
            }
            else
            {
                _rb.AddForce(transform.forward * _walkSpeed, ForceMode.Acceleration);
            }
        }
        if (_jumping)
        {
            _rb.AddForce(Vector3.up * 30, ForceMode.Acceleration);
            _rb.AddForce(transform.forward * 10f, ForceMode.Acceleration);
        }
    }

    private IEnumerator PlayThrustFX()
    {
        var solarPanelRequest = CraftData.GetPrefabForTechTypeAsync(TechType.SolarPanel);
        yield return solarPanelRequest;
        var solarPanelPrefab = solarPanelRequest.GetResult();
        var prefab = Instantiate(solarPanelPrefab.GetComponent<PowerFX>().vfxPrefab);
        var line = prefab.GetComponent<LineRenderer>();
        var newMaterial = new Material(line.material);
        newMaterial.color = new Color(0.2f, 0.8f, 2f);
        line.material = newMaterial;
        line.widthMultiplier = 1;
        line.startWidth = 0.5f;
        line.endWidth = 0.1f;
        line.useWorldSpace = false;
        line.SetPositions(new[] {new Vector3(0, 0, 0), new Vector3(0, -1, -0.5f)});
        var a = Instantiate(prefab, transform);
        a.transform.localPosition = new Vector3(-0.3f, 0, -1.4f);
        a.transform.localRotation = Quaternion.identity;
        var b = Instantiate(prefab, transform);
        b.transform.localPosition = new Vector3(0.3f, 0, -1.4f);
        b.transform.localRotation = Quaternion.identity;
        prefab.SetActive(false);
        Destroy(prefab);
    }
}