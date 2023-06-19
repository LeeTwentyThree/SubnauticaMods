namespace CreatureMorphs.Mono;

public class PossessedCreature : MonoBehaviour
{
    public MorphType morph;
    public Creature creature;
    public SwimBehaviour swimBehaviour;
    public LiveMixin liveMixin;

    private bool _done;

    private float _swimSpeed;

    public static PossessedCreature ControlCreature(GameObject creatureGameObject, MorphType morphType)
    {
        var component = creatureGameObject.AddComponent<PossessedCreature>();
        component.morph = morphType;
        component.creature = creatureGameObject.GetComponent<Creature>();
        component.swimBehaviour = creatureGameObject.GetComponent<SwimBehaviour>();
        component.liveMixin = creatureGameObject.GetComponent<LiveMixin>();
        component._swimSpeed = DetermineSwimSpeed(creatureGameObject);
        var newCreatureAction = creatureGameObject.AddComponent<UnderControlCreatureAction>();
        component.creature.actions.Insert(0, newCreatureAction);
        component.abilities = new List<MorphAbility>();
        foreach (var ability in morphType.morphAbilities)
        {
            var abilityComponent = creatureGameObject.AddComponent(ability.CreatureType);
            ability.PeformSetup(abilityComponent);
        }
        foreach (var onTouch in creatureGameObject.GetComponentsInChildren<OnTouch>())
        {
            onTouch.enabled = false;
        }
        return component;
    }

    private static float DetermineSwimSpeed(GameObject creatureGameObject)
    {
        var sr = creatureGameObject.GetComponent<SwimRandom>();
        if (sr) return sr.swimVelocity;
        var leash = creatureGameObject.GetComponent<StayAtLeashPosition>();
        if (leash) return leash.swimVelocity;
        return 10f;
    }

    public void GainControl()
    {
        _done = true;
    }

    public bool BeingControlled
    {
        get
        {
            return _done;
        }
    }

    private void Update()
    {
        if (!BeingControlled) return;
        swimBehaviour.SwimTo(transform.position + GetInput() * _swimSpeed, _swimSpeed);
    }

    private Vector3 GetInput()
    {
        var moveInput = GameInput.GetMoveDirection();
        var cam = MainCameraControl.main.transform;
        return cam.TransformDirection(moveInput);
    }

    private List<MorphAbility> abilities = new List<MorphAbility>();

    private void OnDestroy()
    {
        PlayerMorpher.main.BecomeHuman();
    }
}
