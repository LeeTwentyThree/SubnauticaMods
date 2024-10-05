using SNCreationKitData.OptionGeneration.Attributes;
using Story;

namespace SNCreationKitData.DataFormats;

public record StoryGoalSettings<T> : DataFormatBase<T> where T : StoryGoal
{
    [InputField("Goal key", "The key of this goal, which determines various aspects of functionality. Must be unique!")]
    public string Key { get; set; }
    [DropDown("Goal type", "The action upon completing this goal.\n" +
                           "PDA: Plays a PDA voice line using the ID from the goal key.\n" +
                           "Radio: Triggers a radio voice line using the ID from the goal key.\n" +
                           "Encyclopedia: Unlocks a Databank entry using the ID from the goal key.\n" +
                           "Story: The default and often preferred type. Has no particular action.",
        typeof(Story.GoalType))]
    public Story.GoalType GoalType { get; set; }
    [InputField("Delay", "The delay in seconds before the goal is triggered.", InputFieldAttribute.ConstraintType.Float)]
    public float Delay { get; set; }

    public override void AssignFieldsToObject(T obj)
    {
        obj.key = Key;
        obj.goalType = GoalType;
        obj.delay = Delay;
    }
}