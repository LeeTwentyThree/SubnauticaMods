using Mono.Cecil;

namespace CreatureMorphs;

internal class MorphBuilder
{
    private MorphType current;

    public void Create(TechType techType, MorphModeType type)
    {
        current = new MorphType(CraftData.GetClassIdForTechType(techType), techType);
        current.morphModeType = type;
    }

    public void SetCameraFollowDistance(float distance)
    {
        current.cameraFollowDistance = distance;
    }

    public void AddAbility<T>(System.Action<T> setup) where T : MorphAbility
    {
        current.morphAbilities.Add(new MorphType.AbilitySetup<T>(setup));
    }

    public MorphType Finish(params TechType[] alternateTechTypes)
    {
        TechType[] all;
        if (alternateTechTypes != null)
        {
            all = alternateTechTypes.Add(current.MainTechType);
        }
        else
        {
            all = new TechType[] { current.MainTechType };
        }
        MorphDatabase.RegisterMorphType(current, all);

        var temp = current;
        current = null;
        return temp;
    }
}
