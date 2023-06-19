using CreatureMorphs.Mono;

namespace CreatureMorphs;

public abstract class MorphAbility : MonoBehaviour
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
}