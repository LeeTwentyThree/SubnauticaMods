using CreatureMorphs.Mono;

namespace CreatureMorphs;
public class MorphType
{
    public MorphType(string creatureClassId)
    {
        MorphClassId = creatureClassId;
        SetEssentials();
    }

    public string MorphClassId { get; private set; }

    public float CameraFollowDistance { get; protected set; } = 5f;

    public MorphModeType MorphModeType { get; protected set; }

    /// <summary>
    /// Use this method to assign permanent fields like <see cref="CameraFollowDistance"/>. Called during patch time.
    /// </summary>
    protected virtual void SetEssentials() { }

    /// <summary>
    /// Use this method to set instance fields on the <paramref name="behaviour"/> component/GameObject. Called at runtime.
    /// </summary>
    /// <param name="behaviour"></param>
    internal virtual void SetupController(MorphInstance behaviour) { }

    protected readonly GameInput.Button PrimaryActionKey = GameInput.Button.LeftHand;
    protected readonly GameInput.Button SecondaryActionKey = GameInput.Button.RightHand;
    protected readonly GameInput.Button MobilityKey = GameInput.Button.Sprint;

    protected static FMODAsset genericBiteSound = Helpers.GetFmodAsset("event:/creature/biter/bite_1");
}
