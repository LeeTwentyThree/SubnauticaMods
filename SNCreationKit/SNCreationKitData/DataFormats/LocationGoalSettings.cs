using SNCreationKitData.OptionGeneration.Attributes;
using Story;
using UnityEngine;

namespace SNCreationKitData.DataFormats;

public record LocationGoalSettings : StoryGoalSettings<LocationGoal>
{
    [InputField("Location name", "The name of this location. Simply for human readability.")]
    public string LocationName { get; set; }
    [Vector3Field("World position", "The position in the world that must be visited for the goal to trigger.")]
    public Vector3 Position { get; set; }
    [InputField("Maximum activation range", "The maximum radius in meters that the player must be from the center position" +
                                            "in order for the goal to be triggered.")]
    public float Range { get; set; }
}