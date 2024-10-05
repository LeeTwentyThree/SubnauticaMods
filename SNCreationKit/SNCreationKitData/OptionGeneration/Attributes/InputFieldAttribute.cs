namespace SNCreationKitData.OptionGeneration.Attributes;

public class InputFieldAttribute : UserOptionAttributeBase
{
    public InputFieldAttribute(string name, string description,
        ConstraintType constraint = ConstraintType.None,
        int characterLimit = -1) : base(name, description)
    {
        Constraint = constraint;
        CharacterLimit = characterLimit;
    }
    
    public ConstraintType Constraint { get; }
    public int CharacterLimit { get; }

    public enum ConstraintType
    {
        None,
        Integer,
        Float,
    }
}