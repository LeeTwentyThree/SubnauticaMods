using UnityEngine;

namespace ModStructureHelperPlugin.Utility;

public static class ModifierFixUtils
{
    public static bool GetModifierHeld(GameInput.Button button)
    {
        var held = GameInput.GetButtonHeld(button);
        
        if (held)
        {
            var binding = GameInput.GetBinding(GameInput.Device.Keyboard, button, GameInput.BindingSet.Primary);
            var token = GetKeyToken(binding);

            switch (token.ToLowerInvariant())
            {
                case "alt":
                case "leftalt":
                    return Input.GetKey(KeyCode.LeftAlt);

                case "rightalt":
                    return Input.GetKey(KeyCode.RightAlt);

                default:
                    return true;
            }
        }

        return held;
    }
    
    private static string GetKeyToken(string binding)
    {
        var slash = binding.LastIndexOf('/');
        return slash >= 0 ? binding.Substring(slash + 1) : binding;
    }
}