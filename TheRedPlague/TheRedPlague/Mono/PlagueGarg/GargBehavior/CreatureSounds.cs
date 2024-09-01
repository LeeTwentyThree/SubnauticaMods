using UnityEngine;

namespace TheRedPlague.Mono.PlagueGarg.GargBehavior;

public class CreatureSounds : MonoBehaviour
{
    public FMOD_CustomEmitter emitter;
    public Creature creature;
    public string animationTriggerParameter;

    public bool updateAnimatorRandom;
    public float minInterval = 8f;
    public float maxInterval = 14f;
    public bool canInterruptSelf = true;

    public FMODAsset closeSound;
    public FMODAsset farSound;
    public float farSoundMinDistance = -1f;

    private float _timePlayAgain;

    public void SetupEssentials(Creature creature, FMOD_CustomEmitter emitter, FMODAsset closeSoundAsset)
    {
        this.creature = creature;
        this.emitter = emitter;
        closeSound = closeSoundAsset;

        emitter.followParent = true;
    }

    public void SetupIntervals(float min, float max)
    {
        minInterval = min;
        maxInterval = max;
    }

    public void SetupFarSound(FMODAsset farSoundAsset, float minDistance)
    {
        farSound = farSoundAsset;
        farSoundMinDistance = minDistance;
    }

    public void SetupAnimation(string animationParameter, bool playRandomAnimations = true)
    {
        animationTriggerParameter = animationParameter;
        updateAnimatorRandom = playRandomAnimations;
    }

    private float TimeWithinInterval()
    {
        return Random.Range(minInterval, maxInterval);
    }

    private void Start()
    {
        _timePlayAgain = Time.time + TimeWithinInterval();
    }

    private void Update()
    {
        if (Time.time > _timePlayAgain)
        {
            PlaySoundOnce(false);
            _timePlayAgain = Time.time + TimeWithinInterval();
        }
    }

    public void PlaySoundOnce(bool forcefullyInterrupt = false)
    {
        if (!forcefullyInterrupt && !canInterruptSelf && emitter.playing)
        {
            return;
        }
        emitter.SetAsset(GetAssetToPlay());
        emitter.Play();
        if (!string.IsNullOrEmpty(animationTriggerParameter))
        {
            creature.GetAnimator().SetTrigger(animationTriggerParameter);
            if (updateAnimatorRandom)
            {
                creature.GetAnimator().SetFloat("random", Random.value);
            }
        }
    }

    private FMODAsset GetAssetToPlay()
    {
        if (farSoundMinDistance < 0f || farSound == null)
        {
            return closeSound;
        }
        var dist = Vector3.Distance(MainCameraControl.main.transform.position, transform.position);
        return dist >= farSoundMinDistance ? farSound : closeSound;
    }
}
