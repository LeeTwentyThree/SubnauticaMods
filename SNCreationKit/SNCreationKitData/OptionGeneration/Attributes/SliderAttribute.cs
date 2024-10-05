namespace SNCreationKitData.OptionGeneration.Attributes;

public class SliderAttribute(string name, string description, float max, float min, float step, string? format)
    : UserOptionAttributeBase(name, description)
{
    public float Max { get; } = max;
    public float Min { get; } = min;
    public float Step { get; } = step;
    public string? Format { get; } = format;
}