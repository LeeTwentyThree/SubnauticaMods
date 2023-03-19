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

    protected abstract void Setup();
    protected abstract void OnInputReceived();

    private System.Func<bool> input;

    public void SetInput(System.Func<bool> input)
    {
        this.input = input;
    }

    public void OnUpdate()
    {
        if (input.Invoke() == true && Time.time >= timeCooldownEnds)
        {
            OnInputReceived();
            timeCooldownEnds = Time.time + abilityCooldown;
        }
    }
}