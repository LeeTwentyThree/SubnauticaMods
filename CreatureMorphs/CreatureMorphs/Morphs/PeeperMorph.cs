using CreatureMorphs.Mono;

namespace CreatureMorphs.Morphs;
internal class PeeperMorph : MorphType
{
    public PeeperMorph(string morphClassId) : base(morphClassId)
    {
    }

    protected override void SetEssentials()
    {
        CameraFollowDistance = 4f;
        MorphModeType = MorphModeType.Prey;
    }

    internal override void SetupController(MorphInstance controller)
    {
        controller.AddAbility(new Bite(4f, null, genericBiteSound), PrimaryActionKey);
    }
}