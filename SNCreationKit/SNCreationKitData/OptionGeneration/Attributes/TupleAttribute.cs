namespace SNCreationKitData.OptionGeneration.Attributes;

public class TupleAttribute : UserOptionAttributeBase
{
    public TupleAttribute(string name, string description, Type[] elementTypes) : base(name, description)
    {
        ElementTypes = elementTypes;
    }
    
    public TupleAttribute(string name, string description, Type elementType, int num) : base(name, description)
    {
        ElementTypes = (Type[])Array.CreateInstance(elementType, num);
    }

    public Type[] ElementTypes { get; }
}