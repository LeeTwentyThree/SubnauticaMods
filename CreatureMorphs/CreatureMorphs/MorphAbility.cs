using CreatureMorphs.Mono;

namespace CreatureMorphs;

internal abstract class MorphAbility : MonoBehaviour
{
    public float abilityCooldown;

    protected float timeCooldownEnds;

    public PossessedCreature morphController;

    public void ProcessInput()
    {
        if (Time.time >= timeCooldownEnds)
        {
            OnInputReceived();
            timeCooldownEnds = Time.time + abilityCooldown;
        }
    }

    protected abstract void OnInputReceived();

    public abstract string GetName();
}