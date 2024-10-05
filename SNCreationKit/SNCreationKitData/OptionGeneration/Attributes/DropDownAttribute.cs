namespace SNCreationKitData.OptionGeneration.Attributes;

public class DropDownAttribute : UserOptionAttributeBase
{
    public DropDownAttribute(string name, string description, Type enumType) : base(name, description)
    {
        Mode = DropDownMode.Enum;
        EnumType = enumType;
    }

    public DropDownAttribute(string name, string description, string[] values) : base(name, description)
    {
        Mode = DropDownMode.Static;
        Values = values;
    }
    
    public DropDownMode Mode { get; }
    public Type? EnumType { get; }
    public string[]? Values { get; }
    
    public enum DropDownMode
    {
        Static,
        Enum
    }
}