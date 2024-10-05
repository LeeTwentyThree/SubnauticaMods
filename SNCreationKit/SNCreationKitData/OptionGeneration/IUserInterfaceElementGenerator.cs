namespace SNCreationKitData.OptionGeneration;

public interface IUserInterfaceElementGenerator<TContainer>
{
    public void GenerateUserInterface(IUserInterfaceManager<TContainer> ui);
}