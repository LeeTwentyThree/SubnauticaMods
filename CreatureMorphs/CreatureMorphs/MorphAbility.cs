using CreatureMorphs.Mono;

namespace CreatureMorphs;
internal abstract class MorphAbility
{
    public float abilityCooldown;

    protected float timeCooldownEnds;

    public MorphInstance morphController;

    public MorphAbility()
    {
        Setup();
    }

    public void ProcessInput()
    {
        if (Time.time >= timeCooldownEnds)
        {
            OnInputReceived();
            timeCooldownEnds = Time.time + abilityCooldown;
        }

    }
    protected abstract void Setup();
    protected abstract void OnInputReceived();
}