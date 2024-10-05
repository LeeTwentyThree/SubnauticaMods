namespace SNCreationKitData.OptionGeneration;

public interface IPropertyOptionGenerator<in TAttribute, TContainer> : IUserInterfaceElementGenerator<TContainer>
    where TAttribute : UserOptionAttributeBase
{
    public Type OptionType { set; }
    public TAttribute DataAttribute { set; }
}