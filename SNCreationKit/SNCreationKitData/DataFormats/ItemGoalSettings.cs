using SNCreationKitData.OptionGeneration.Attributes;
using Story;

namespace SNCreationKitData.DataFormats;

public record ItemGoalSettings : StoryGoalSettings<ItemGoal>
{
    [DropDown("Required item TechType", "The TechType of the item that must be picked up to complete this goal.", typeof(TechType))]
    public TechType ItemTechType { get; set; }
    
    public override void AssignFieldsToObject(ItemGoal obj)
    {
        base.AssignFieldsToObject(obj);
        obj.techType = ItemTechType;
    }
}