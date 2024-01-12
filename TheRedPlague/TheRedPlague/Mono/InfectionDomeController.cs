using System.Linq;
using Nautilus.Utility;
using Story;
using UnityEngine;

namespace TheRedPlague.Mono;

public class InfectionDomeController : MonoBehaviour
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

    private void Awake()
    {
        main = this;
        ResetTimer();
    }

    public void OnBeginRocketAnimation()
    {
        _timeStartRocketAnimation = Time.time;
        _rocketAnimation = true;
    }

    private void Update()
    {
        if (PlagueHeartBehavior.main == null)
        {
            return;
        }
        
        if (Time.time > _timeStrikeAgain)
        {
            if (PlagueHeartBehavior.main.isActiveAndEnabled && TryGetRandomTarget(out var target))
            {
                StrikeTarget(target);
                if (Vector3.SqrMagnitude(Player.main.transform.position - PlagueHeartBehavior.main.transform.position) < 24 * 24)
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
            Destroy(this);
            return;
        }
        
        var plagueHeart = PlagueHeartBehavior.main;
        if (plagueHeart == null)
            return;
        var positionA = plagueHeart.transform.position;
        var positionB = GetRandomDomePosition();
        var positionC = target.transform.position;
        
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
        if (PlagueHeartBehavior.main != null && Vector3.SqrMagnitude(Player.main.transform.position - PlagueHeartBehavior.main.transform.position) < 24 * 24)
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
}