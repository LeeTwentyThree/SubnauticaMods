namespace CreatureMorphs.Mono
{
    internal class MorphInstance : MonoBehaviour
    {
        public MorphType morph;
        public Creature creature;
        public SwimBehaviour swimBehaviour;
        public LiveMixin liveMixin;

        private bool _done;

        private float _swimSpeed;

        public static MorphInstance ControlCreature(GameObject creatureGameObject, MorphType morphType)
        {
            var component = creatureGameObject.AddComponent<MorphInstance>();
            component.morph = morphType;
            component.creature = creatureGameObject.GetComponent<Creature>();
            component.swimBehaviour = creatureGameObject.GetComponent<SwimBehaviour>();
            component.liveMixin = creatureGameObject.GetComponent<LiveMixin>();
            component._swimSpeed = DetermineSwimSpeed(creatureGameObject);
            var newCreatureAction = creatureGameObject.AddComponent<UnderControlCreatureAction>();
            component.creature.actions.Insert(0, newCreatureAction);
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

        private void Start()
        {
            morph.SetupController(this);
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
            foreach (var ability in abilities)
            {
                ability.OnUpdate();
            }
        }

        private Vector3 GetInput()
        {
            var moveInput = GameInput.GetMoveDirection();
            var cam = MainCameraControl.main.transform;
            return cam.TransformDirection(moveInput);
        }

        private List<MorphAbility> abilities = new List<MorphAbility>();

        public MorphAbility AddAbility(MorphAbility ability, KeyCode input)
        {
            abilities.Add(ability);
            if (input != KeyCode.None) ability.SetInput(() => Input.GetKeyDown(input));
            ability.morphController = this;
            return ability;
        }

        public MorphAbility AddAbility(MorphAbility ability, GameInput.Button input)
        {
            abilities.Add(ability);
            ability.SetInput(() => GameInput.GetButtonDown(input));
            ability.morphController = this;
            return ability;
        }

        public MorphAbility AddAbility(MorphAbility ability, System.Func<bool> input)
        {
            abilities.Add(ability);
            ability.SetInput(input);
            ability.morphController = this;
            return ability;
        }

        private void OnDestroy()
        {
            Morphing.main.BecomeHuman();
        }
    }
}
