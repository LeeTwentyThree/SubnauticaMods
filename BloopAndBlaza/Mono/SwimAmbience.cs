using UnityEngine;
using System.Collections;
using Nautilus.Utility;

namespace BloopAndBlaza.Mono;

public class SwimAmbience : MonoBehaviour
{
    public int emitterCount = 3;
    public float delay = 2f;

    private static readonly FMODAsset SwimSound = AudioUtils.GetFmodAsset("BloopSwim");
    private FMOD_CustomEmitter[] _myEmitters;

    private int _currentEmitterIndex;

    private void Awake()
    {
        _myEmitters = new FMOD_CustomEmitter[emitterCount];
        for (int i = 0; i < emitterCount; i++)
        {
            _myEmitters[i] = AddEmitter();
        }
    }

    private FMOD_CustomEmitter AddEmitter()
    {
        var emitter = gameObject.AddComponent<FMOD_CustomEmitter>();
        emitter.followParent = true;
        emitter.SetAsset(SwimSound);
        return emitter;
    }

    private FMOD_CustomEmitter CycleEmitters()
    {
        _currentEmitterIndex++;
        if (_currentEmitterIndex >= emitterCount)
        {
            _currentEmitterIndex = 0;
        }

        return _myEmitters[_currentEmitterIndex];
    }

    private IEnumerator Start()
    {
        for (;;)
        {
            yield return new WaitForSeconds(delay);
            var nextEmitter = CycleEmitters();
            nextEmitter.Play();
        }
    }
}