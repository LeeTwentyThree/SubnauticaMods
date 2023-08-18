using Nautilus.Json;
using Nautilus.Options.Attributes;

namespace CustomWaterLevel
{
    [Menu("Custom Water Level")]
    public class WaterLevelConfig : ConfigFile
    {
        [Slider(Label = "Water level", Tooltip = "The level of the water, in meters, relative to the default water level.\nRESTART REQUIRED FOR CERTAIN FEATURES TO TAKE EFFECT.", DefaultValue = 0, Min = -1250, Max = 500, Step = 5)]
        public float WaterLevel = 0f;
        [Toggle(Label = "Suffocate fish", Tooltip = "Whether fish should suffocate when above water or not.")]
        public bool SuffocateFish = false;
        [Toggle(Label = "Remove ALL shoals of fish", Tooltip = "Whether ALL shoals of fish (regardless of depth) should be removed from the game. They may ruin the immersion.")]
        public bool RemoveSchoolsOfFish = false;
        [Toggle(Label = "Mobile Vehicle Bay fix", Tooltip = "If enabled, allows you to deploy Mobile Vehicle Bays on land.")]
        public bool FixConstructor = true;
        [Toggle(Label = "Improve PRAWN air mobility", Tooltip = "If enabled, allows the PRAWN Suit to move freely in water AND on land.")]
        public bool BuffExosuit = false;
        [Toggle(Label = "Enable automatic mode", Tooltip = "If enabled, the water level will change on its own, and will be influenced by the settings below.\nRESTART REQUIRED.")]
        public bool AutomaticChange = false;
        [Slider(Label = "Time between movements", Tooltip = "The amount of time, in seconds, between each water level change.\nFOR AUTOMATIC MODE ONLY.", DefaultValue = 60, Min = 0, Max = 600, Step = 5)]
        public float IntervalDuration = 60f;
        [Slider(Label = "Distance per movement", Tooltip = "The distance in meters that the water will move during each change, can be positive or negative.\nFOR AUTOMATIC MODE ONLY.", DefaultValue = 0, Min = -25, Max = 25, Step = 5)]
        public float IntervalChange = 0f;
        [Slider(Label = "Water vertical speed", Tooltip = "The speed, in 1/4 meters per second, of which the ocean level rises/falls.\nFOR AUTOMATIC MODE ONLY.", DefaultValue = 4f, Min = 1f, Max = 16f, Step = 1f)]
        public float WaterMoveSpeed = 4f;
    }
}