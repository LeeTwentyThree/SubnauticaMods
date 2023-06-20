namespace CreatureMorphs;

internal class MorphBuilder
{
    private MorphType current;

    public void Create(TechType techType, MorphAnimationType type, float radius = 2f)
    {
        current = new(CraftData.GetClassIdForTechType(techType), techType);
        current.morphModeType = type;
        current.sphereCheckRadius = radius;
    }

    public void SetCameraFollowDistance(float distance)
    {
        current.cameraFollowDistance = distance;
    }

    public void AddAbility<T>(System.Action<T> setup) where T : MorphAbility
    {
        current.morphAbilities.Add(new MorphType.AbilitySetup<T>(setup));
    }

    public void SetSwimSpeed(float swimSpeed)
    {
        current.overrideSwimSpeed = swimSpeed;
    }

    public void SetCameraPositionOffset(Vector3 offset)
    {
        current.cameraPositionOffset = offset;
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
