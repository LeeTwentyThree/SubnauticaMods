using System.Collections.Generic;
using SNCreationKitData.OptionGeneration;
using UnityEngine;

namespace SNCreationKitPlugin.OptionsImplementation;

public class UserInterfaceManager : IUserInterfaceManager<RectTransform>
{
    private Stack<InterfaceContext> _contextStack;
    private InterfaceContext _currentContext;
    
    public RectTransform CreateContainer()
    {
        
    }

    public void OpenContext(InterfaceContext context)
    {
        _contextStack.Push(context);
        _currentContext = context;
    }

    public void CloseContext(InterfaceContext context)
    {
        if (_contextStack.Peek() != context)
        {
            throw new InvalidContextException(_contextStack.Peek(), context);
        }
        _contextStack.Pop();
        if (_contextStack.Count == 0)
            _currentContext = InterfaceContext.None;
        else _currentContext = _contextStack.Peek();
    }

    public void CreateHeader(string text)
    {
        
    }

    private class InvalidContextException : System.Exception
    {
        public InvalidContextException(InterfaceContext currentContext, InterfaceContext inputContext) : 
            base($"Attempting to leave '{inputContext}' context while currently in '{currentContext}'.") {}
    }
}