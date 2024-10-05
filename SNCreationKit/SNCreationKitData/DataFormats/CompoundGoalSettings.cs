using SNCreationKitData.OptionGeneration.Attributes;
using Story;

namespace SNCreationKitData.DataFormats;

public record CompoundGoalSettings : StoryGoalSettings<CompoundGoal>
{
    [List("Precondition goals", "The keys of all goals that must be completed before this goal can be acquired.")]
    [InputField("Goal key", "A required goal key.")]
    public string[] Preconditions { get; }

    public override void AssignFieldsToObject(CompoundGoal obj)
    {
        base.AssignFieldsToObject(obj);
        obj.preconditions = Preconditions;
    }
}