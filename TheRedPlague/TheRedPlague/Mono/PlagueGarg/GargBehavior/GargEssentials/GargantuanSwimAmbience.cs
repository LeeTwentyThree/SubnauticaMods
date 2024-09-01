using System.Collections;
using FMOD;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono.PlagueGarg.GargBehavior.GargEssentials;

internal class GargantuanSwimAmbience : MonoBehaviour
{
    public string eventPath;
    public float delay = 26.742f;
    public int emittersCount = 2;
    public float delayVariation;
    
    private FMOD_CustomEmitter[] _emitters;

    private void Awake()
    {
        _emitters = new FMOD_CustomEmitter[emittersCount];
        for (int i = 0; i < emittersCount; i++)
        {
            _emitters[i] = AddEmitter();
        }

    }

    private IEnumerator Start()
    {
        for (; ; )
        {
            var emitter = GetAvailableEmitter();
            if (emitter != null) emitter.Play();
            yield return new WaitForSeconds(delayVariation > Mathf.Epsilon ? delay + Random.Range(-delayVariation, delayVariation) : delay);
        }
    }

    private FMOD_CustomEmitter AddEmitter()
    {
        var emitter = gameObject.AddComponent<FMOD_CustomEmitter>();
        emitter.playOnAwake = false;
        emitter.followParent = true;
        emitter.SetAsset(AudioUtils.GetFmodAsset(eventPath));
        return emitter;
    }

    private FMOD_CustomEmitter GetAvailableEmitter()
    {
        for (int i = 0; i < emittersCount; i++)
        {
            if (!GetIsPlaying(_emitters[i]))
            {
                return _emitters[i];
            }
        }
        return null;
    }

    private bool GetIsPlaying(FMOD_CustomEmitter emitter)
    {
        if (!CustomSoundHandler.TryGetCustomSoundChannel(emitter.GetInstanceID(), out var channel)) return false;
        return channel.isPlaying(out var playing) == RESULT.OK && playing;
    }
}