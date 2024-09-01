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
    
    private bool _rocketAnimation;
    private float _timeStartRocketAnimation;
    private float _moveUpwardsDelay = 26;
    private float _rocketVelocity;
    private float _rocketAccel = 40;
    
    private void Awake()
    {
        main = this;
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
        if (_rocketAnimation && Time.time > _timeStartRocketAnimation + _moveUpwardsDelay)
        {
            _rocketVelocity += Time.deltaTime * _rocketAccel;
            transform.position += Vector3.down * (_rocketVelocity * Time.deltaTime);
        }
    }

    public void NotifyGoalComplete(string key)
    {
        if (key == StoryUtils.InfectCables.key)
        {
            SetDomeDisabled();
        }
        else if (key == StoryUtils.DisableDome.key)
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
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        var gibs = transform.GetChild(0).GetChild(1).gameObject;
        gibs.SetActive(true);
        foreach (var rb in gibs.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddExplosionForce(100000f, Vector3.zero, 5000, 0.2f);
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }
}