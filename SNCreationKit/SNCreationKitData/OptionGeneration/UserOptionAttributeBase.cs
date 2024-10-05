namespace SNCreationKitData.OptionGeneration;

public abstract class UserOptionAttributeBase(string name, string description) : Attribute
{
    public string Name { get; } = name;
    public string Description { get; } = description;
}