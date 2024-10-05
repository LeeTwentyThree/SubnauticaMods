using UnityEngine;

namespace SNCreationKitData.OptionGeneration;

public interface IUserInterfaceManager<TContainer>
{
    public TContainer CreateContainer();
    public void OpenContext(InterfaceContext context);
    public void CloseContext(InterfaceContext context);
    public void CreateHeader(string text);
    
}