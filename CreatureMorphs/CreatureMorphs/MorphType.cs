using CreatureMorphs.Mono;
using System;

namespace CreatureMorphs;

internal class MorphType
{
    public MorphType(string creatureClassId, TechType techType)
    {
        MorphClassId = creatureClassId;
        MainTechType = techType;
    }

    public TechType MainTechType { get; protected set; }
    public string MorphClassId { get; private set; }

    public float cameraFollowDistance = 5f;

    public MorphAnimationType morphModeType;

    public float? overrideSwimSpeed;

    public Vector3 cameraPositionOffset;

    public float sphereCheckRadius;

    public readonly List<AbilitySetupBase> morphAbilities = new List<AbilitySetupBase>();

    protected static FMODAsset genericBiteSound = Helpers.GetFmodAsset("event:/creature/biter/bite_1");

    public abstract class AbilitySetupBase
    {
        public abstract Type CreatureType { get; }
        public abstract void PeformSetup(object component);
    }

    public class AbilitySetup<T> : AbilitySetupBase where T : MorphAbility
    {
        public Action<T> action;

        public AbilitySetup(Action<T> action)
        {
            this.action = action;
        }

        public override Type CreatureType => typeof(T);

        public override void PeformSetup(object component)
        {
            if (component is T abilityComponent)
            {
                action.Invoke(abilityComponent);
            }
        }
    }
}