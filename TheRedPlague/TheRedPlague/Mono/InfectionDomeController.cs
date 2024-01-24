using System;
using System.Linq;
using mset;
using Nautilus.Handlers;
using Nautilus.Utility;
using Story;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TheRedPlague.Mono;

public class InfectionDomeController : MonoBehaviour, IStoryGoalListener
{
    public static InfectionDomeController main;
    
    public GameObject linePrefab;

    private float _timeStrikeAgain;
    private float _minInterval = 2;
    private float _maxInterval = 12;

    private void ResetTimer() => _timeStrikeAgain = Time.time + Random.Range(_minInterval, _maxInterval);

    private static FMODAsset _strikeTargetSound = AudioUtils.GetFmodAsset("InfectionLaserShot");

    private bool _rocketAnimation;
    private float _timeStartRocketAnimation;
    private float _moveUpwardsDelay = 26;
    private float _rocketVelocity;
    private float _rocketAccel = 40;

    private bool _playerWasNearby;

    private void Awake()
    {
        main = this;
        ResetTimer();
    }

    private void Start()
    {
        StoryGoalManager.main.AddListener(this);
        if (StoryGoalManager.main.IsGoalComplete(StoryUtils.DisableDome.key))
        {
            SetDomeColor(new Color(0, 0, 0));
        }
        else if (StoryGoalManager.main.IsGoalComplete(StoryUtils.EnzymeRainEnabled.key))
        {
            SetDomeColor(new Color(0.5f, 1, 0.5f));
        }
    }

    private void SetDomeColor(Color color)
    {
        var materials = GetComponentInChildren<Renderer>().materials;
        materials[2].color = color;
    }

    public void OnBeginRocketAnimation()
    {
        _timeStartRocketAnimation = Time.time;
        _rocketAnimation = true;
    }

    private void Update()
    {
        if (PlagueHeartBehavior.main != null && Time.time > _timeStrikeAgain)
        {
            if (PlagueHeartBehavior.main.isActiveAndEnabled && TryGetRandomTarget(out var target))
            {
                StrikeTarget(target);
                if (_playerWasNearby)
                {
                    _timeStrikeAgain = Time.time + Random.value + 0.4f;
                }
                else
                {
                    ResetTimer();
                }
            }
        }

        if (_rocketAnimation && Time.time > _timeStartRocketAnimation + _moveUpwardsDelay)
        {
            _rocketVelocity += Time.deltaTime * _rocketAccel;
            transform.position += Vector3.down * (_rocketVelocity * Time.deltaTime);
        }
    }

    private void StrikeTarget(InfectionStrikeTarget target)
    {
        if (StoryGoalManager.main.IsGoalComplete(StoryUtils.ForceFieldLaserDisabled.key))
        {
            return;
        }
        
        var plagueHeart = PlagueHeartBehavior.main;
        if (plagueHeart == null)
            return;
        
        var positionA = plagueHeart.transform.position;
        var positionB = GetRandomDomePosition();
        var positionC = target.transform.position;
        
        if (_playerWasNearby)
        {
            var line = Instantiate(linePrefab);
            AddLerpColors(line);
            line.SetActive(true);
            line.GetComponent<LineRenderer>().SetPositions(new[] {positionA, positionC});
            Destroy(line, 1);
        }
        else
        {
            var line1 = Instantiate(linePrefab);
            AddLerpColors(line1);
            line1.SetActive(true);
            line1.GetComponent<LineRenderer>().SetPositions(new[] {positionA, positionB});
            Destroy(line1, 1);

            var line2 = Instantiate(linePrefab);
            AddLerpColors(line2);
            line2.SetActive(true);
            line2.GetComponent<LineRenderer>().SetPositions(new[] {positionB, positionC});
            Destroy(line2, 1);
        }
        
        target.Infect();
        Utils.PlayFMODAsset(_strikeTargetSound, target.transform.position);
    }

    private void AddLerpColors(GameObject line)
    {
        var lerp = line.AddComponent<VFXLerpColor>();
        lerp.duration = 1f;
        lerp.blendCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        lerp.colorEnd = new Color(0, 0, 0, 0);
    }

    private Vector3 GetRandomDomePosition()
    {
        var scaled = Random.onUnitSphere * 2000;
        return new Vector3(scaled.x, Mathf.Abs(scaled.y), scaled.z);
    }

    private bool TryGetRandomTarget(out InfectionStrikeTarget chosenTarget)
    {
        _playerWasNearby = Vector3.SqrMagnitude(Player.main.transform.position - PlagueHeartBehavior.main.transform.position) < 9 * 9;
        if (PlagueHeartBehavior.main != null && _playerWasNearby)
        {
            chosenTarget = Player.main.gameObject.EnsureComponent<InfectionStrikeTarget>();
            return true;
        }
        
        var validTargets = InfectionStrikeTarget.AllTargets.Where(target => target.IsValidTarget()).ToArray();

        if (validTargets.Length == 0)
        {
            chosenTarget = null;
            return false;
        }

        chosenTarget = validTargets[Random.Range(0, validTargets.Length)];
        return chosenTarget != null;
    }

    public void NotifyGoalComplete(string key)
    {
        if (key == StoryUtils.DisableDome.key)
        {
            Invoke(nameof(SetDomeDisabled), 13);
        }
        else if (key == StoryUtils.EnzymeRainEnabled.key)
        {
            Invoke(nameof(SetDomeGreen), 13);
        }
    }
    
    private void SetDomeDisabled() => SetDomeColor(new Color(0, 0, 0, 0));
    private void SetDomeGreen() => SetDomeColor(new Color(0.5f, 1, 0.5f));
    
    private void OnDestroy()
    {
        StoryGoalManager main = StoryGoalManager.main;
        if (main)
        {
            main.RemoveListener(this);
        }
    }

    public void ShatterDome()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        var gibs = transform.GetChild(1).gameObject;
        gibs.SetActive(true);
        foreach (var rb in gibs.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddExplosionForce(100000f, Vector3.zero, 5000, 0.2f);
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }
}