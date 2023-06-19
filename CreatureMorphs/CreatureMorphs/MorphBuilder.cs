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
        List<TechType> all = new List<TechType>() { current.MainTechType };
        if (alternateTechTypes != null)
        {
            all.AddRange(alternateTechTypes);
        }
        MorphDatabase.RegisterMorphType(current, all.ToArray());

        var temp = current;
        current = null;
        return temp;
    }
}
