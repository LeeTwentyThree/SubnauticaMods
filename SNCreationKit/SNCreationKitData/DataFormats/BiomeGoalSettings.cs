using SNCreationKitData.OptionGeneration.Attributes;
using Story;

namespace SNCreationKitData.DataFormats;

public record BiomeGoalSettings : StoryGoalSettings<BiomeGoal>
{
    [InputField("Biome name", "The name of the biome that must be entered for this goal to trigger.")]
    public string BiomeName { get; }
    [InputField("Minimum stay duration", "The minimum number of seconds the player must stay " +
                                         "in the biome in order for this goal to trigger.", InputFieldAttribute.ConstraintType.Float)]
    public float MinStayDuration { get; }

    public override void AssignFieldsToObject(BiomeGoal obj)
    {
        base.AssignFieldsToObject(obj);
        obj.biome = BiomeName;
        obj.minStayDuration = MinStayDuration;
    }
}