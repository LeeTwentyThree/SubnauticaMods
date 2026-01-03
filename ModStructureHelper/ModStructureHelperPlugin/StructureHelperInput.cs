using System;
using System.Reflection;
using Nautilus.Handlers;

namespace ModStructureHelperPlugin;

public static class StructureHelperInput
{
    private const string GeneralCategory = "StructureHelperGeneral";
    private const string ToolsCategory = "StructureHelperTools";
    private const string ModifiersCategory = "StructureHelperModifiers";
    private const string OtherCategory = "StructureHelperOther";

    internal static void RegisterLocalization()
    {
        var fields = typeof(StructureHelperInput).GetFields();
        foreach (var field in fields)
        {
            var translationAttribute = field.GetCustomAttribute<EnglishTranslationAttribute>();
            if (translationAttribute == null)
            {
                Plugin.Logger.LogWarning("Missing English translation for binding " + field.Name);
                continue;
            }

            var buttonValue = (GameInput.Button)field.GetValue(null);
            LanguageHandler.SetLanguageLine("Option" + buttonValue, translationAttribute.Translation);
        }
        
        LanguageHandler.SetLanguageLine(GeneralCategory, "<u>Mod Structure Helper</u>: General");
        LanguageHandler.SetLanguageLine(ToolsCategory, "<u>Mod Structure Helper</u>: Tools");
        LanguageHandler.SetLanguageLine(ModifiersCategory, "<u>Mod Structure Helper</u>: Modifiers");
        LanguageHandler.SetLanguageLine(OtherCategory, "<u>Mod Structure Helper</u>: Other");
    }
    
    // GENERAL
    [EnglishTranslation("Toggle structure helper")]
    public static readonly GameInput.Button ToggleStructureHelperKeyBind = EnumHandler
        .AddEntry<GameInput.Button>("SH_ToggleStructureHelperKeyBind")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.F4)
        .AvoidConflicts()
        .WithCategory(GeneralCategory);

    [EnglishTranslation("Save (w/ modifier)")]
    public static readonly GameInput.Button SaveKeyBind = EnumHandler
        .AddEntry<GameInput.Button>("SH_SaveKeyBind")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.S)
        .AvoidConflicts()
        .WithCategory(GeneralCategory);
    
    [EnglishTranslation("Interact")]
    public static readonly GameInput.Button Interact = EnumHandler
        .AddEntry<GameInput.Button>("SH_Interact")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Mouse.LeftButton)
        .AvoidConflicts()
        .WithCategory(GeneralCategory);

    // TOOLS
    [EnglishTranslation("Activate select mode")]
    public static readonly GameInput.Button SelectBind = EnumHandler
        .AddEntry<GameInput.Button>("SH_SelectBind")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.Q)
        .AvoidConflicts()
        .WithCategory(ToolsCategory);

    [EnglishTranslation("Translate")]
    public static readonly GameInput.Button TranslateBind = EnumHandler
        .AddEntry<GameInput.Button>("SH_TranslateBind")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.E)
        .AvoidConflicts()
        .WithCategory(ToolsCategory);

    [EnglishTranslation("Rotate")]
    public static readonly GameInput.Button RotateBind = EnumHandler
        .AddEntry<GameInput.Button>("SH_RotateBind")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.R)
        .AvoidConflicts()
        .WithCategory(ToolsCategory);

    [EnglishTranslation("Scale")]
    public static readonly GameInput.Button ScaleBind = EnumHandler
        .AddEntry<GameInput.Button>("SH_ScaleBind")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.T)
        .AvoidConflicts()
        .WithCategory(ToolsCategory);

    [EnglishTranslation("Activate drag & drop tool")]
    public static readonly GameInput.Button DragBind = EnumHandler
        .AddEntry<GameInput.Button>("SH_DragBind")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.F)
        .AvoidConflicts()
        .WithCategory(ToolsCategory);

    [EnglishTranslation("Open entity browser")]
    public static readonly GameInput.Button EntityEditorBind = EnumHandler
        .AddEntry<GameInput.Button>("SH_EntityEditorBind")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.Tab)
        .AvoidConflicts()
        .WithCategory(ToolsCategory);

    [EnglishTranslation("Activate brush tool")]
    public static readonly GameInput.Button PaintBrushBind = EnumHandler
        .AddEntry<GameInput.Button>("SH_PaintBrushBind")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.B)
        .AvoidConflicts()
        .WithCategory(ToolsCategory);

    [EnglishTranslation("Toggle global space")]
    public static readonly GameInput.Button ToggleGlobalSpaceBind = EnumHandler
        .AddEntry<GameInput.Button>("SH_ToggleGlobalSpaceBind")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.G)
        .AvoidConflicts()
        .WithCategory(ToolsCategory);

    [EnglishTranslation("Toggle snapping")]
    public static readonly GameInput.Button ToggleSnappingBind = EnumHandler
        .AddEntry<GameInput.Button>("SH_ToggleSnappingBind")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.P)
        .AvoidConflicts()
        .WithCategory(ToolsCategory);

    [EnglishTranslation("Pick object for brushing")]
    public static readonly GameInput.Button PickObjectBind = EnumHandler
        .AddEntry<GameInput.Button>("SH_PickObjectBind")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.K)
        .AvoidConflicts()
        .WithCategory(ToolsCategory);
    
    [EnglishTranslation("Quick pick entity")]
    public static readonly GameInput.Button QuickPickEntity = EnumHandler
        .AddEntry<GameInput.Button>("SH_QuickPickEntityBind")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Mouse.MiddleButton)
        .AvoidConflicts()
        .WithCategory(ToolsCategory);

    [EnglishTranslation("Toggle cable editor")]
    public static readonly GameInput.Button CableEditorBind = EnumHandler
        .AddEntry<GameInput.Button>("SH_CableEditorBind")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.M)
        .AvoidConflicts()
        .WithCategory(ToolsCategory);

    [EnglishTranslation("Duplicate (w/ modifier)")]
    public static readonly GameInput.Button DuplicateBind = EnumHandler
        .AddEntry<GameInput.Button>("SH_DuplicateBind")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.D)
        .AvoidConflicts()
        .WithCategory(ToolsCategory);

    [EnglishTranslation("Select all (w/ modifier)")]
    public static readonly GameInput.Button SelectAllBind = EnumHandler
        .AddEntry<GameInput.Button>("SH_SelectAllBind")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.H)
        .AvoidConflicts()
        .WithCategory(ToolsCategory);

    [EnglishTranslation("Undo (w/ modifier)")]
    public static readonly GameInput.Button UndoBind = EnumHandler
        .AddEntry<GameInput.Button>("SH_UndoBind")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.Z)
        .AvoidConflicts()
        .WithCategory(ToolsCategory);

    [EnglishTranslation("Select last selected (w/ alt modifier)")]
    public static readonly GameInput.Button SelectLastSelectedBind = EnumHandler
        .AddEntry<GameInput.Button>("SH_SelectLastSelectedBind")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.Z)
        .AvoidConflicts()
        .WithCategory(ToolsCategory);
    
    [EnglishTranslation("Delete")]
    public static readonly GameInput.Button DeleteBind = EnumHandler
        .AddEntry<GameInput.Button>("SH_DeleteBind")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.Delete)
        .AvoidConflicts()
        .WithCategory(ToolsCategory);
    
    // MODIFIERS
    
    [EnglishTranslation("Prioritize triggers when selecting")]
    public static readonly GameInput.Button PrioritizeTriggers = EnumHandler
        .AddEntry<GameInput.Button>("SH_PrioritizeTriggers")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.Alt)
        .AvoidConflicts()
        .WithCategory(ModifiersCategory);

    [EnglishTranslation("Scale uniformly modifier")]
    public static readonly GameInput.Button ScaleUniform = EnumHandler
        .AddEntry<GameInput.Button>("SH_ScaleUniform")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.Alt)
        .AvoidConflicts()
        .WithCategory(ModifiersCategory);
    
    [EnglishTranslation("Save hotkey modifier")]
    public static readonly GameInput.Button SaveHotkeyModifier = EnumHandler
        .AddEntry<GameInput.Button>("SH_SaveHotkeyModifier")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.LeftCtrl)
        .AvoidConflicts()
        .WithCategory(ModifiersCategory);
    
    [EnglishTranslation("Control tool modifier")]
    public static readonly GameInput.Button ToolHotkeyModifier = EnumHandler
        .AddEntry<GameInput.Button>("SH_ToolHotkeyModifier")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.LeftCtrl)
        .AvoidConflicts()
        .WithCategory(ModifiersCategory);
    
    [EnglishTranslation("Alt tool modifier")]
    public static readonly GameInput.Button AltToolHotkeyModifier = EnumHandler
        .AddEntry<GameInput.Button>("SH_AltToolHotkeyModifier")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.LeftAlt)
        .AvoidConflicts()
        .WithCategory(ModifiersCategory);
    
    [EnglishTranslation("Select: select multiple")]
    public static readonly GameInput.Button SelectMultipleModifier = EnumHandler
        .AddEntry<GameInput.Button>("SH_SelectMultipleModifier")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.LeftCtrl)
        .AvoidConflicts()
        .WithCategory(ModifiersCategory);

    // OTHER
    
    [EnglishTranslation("Brush: rotate left")]
    public static readonly GameInput.Button BrushRotateLeft = EnumHandler
        .AddEntry<GameInput.Button>("SH_BrushRotateLeft")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.LeftBracket, GameInputHandler.Paths.Mouse.ScrollUp)
        .AvoidConflicts()
        .WithCategory(OtherCategory);

    [EnglishTranslation("Brush: rotate right")]
    public static readonly GameInput.Button BrushRotateRight = EnumHandler
        .AddEntry<GameInput.Button>("SH_BrushRotateRight")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.RightBracket, GameInputHandler.Paths.Mouse.ScrollDown)
        .AvoidConflicts()
        .WithCategory(OtherCategory);
    
    [EnglishTranslation("Brush: decrease scale")]
    public static readonly GameInput.Button BrushDecreaseScale = EnumHandler
        .AddEntry<GameInput.Button>("SH_BrushDecreaseScale")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.Minus)
        .AvoidConflicts()
        .WithCategory(OtherCategory);

    [EnglishTranslation("Brush: increase scale")]
    public static readonly GameInput.Button BrushIncreaseScale = EnumHandler
        .AddEntry<GameInput.Button>("SH_BrushIncreaseScale")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.Equals)
        .AvoidConflicts()
        .WithCategory(OtherCategory);

    [EnglishTranslation("Brush/drag: use global up normal")]
    public static readonly GameInput.Button UseGlobalUpNormal = EnumHandler
        .AddEntry<GameInput.Button>("SH_UseGlobalUpNormal")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Keyboard.Semicolon)
        .AvoidConflicts()
        .WithCategory(OtherCategory);
    
    [EnglishTranslation("Browser: go back")]
    public static readonly GameInput.Button GoBack = EnumHandler
        .AddEntry<GameInput.Button>("SH_GoBack")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Mouse.BackButton)
        .AvoidConflicts()
        .WithCategory(OtherCategory);
    
    [EnglishTranslation("Browser: go forward")]
    public static readonly GameInput.Button GoForward = EnumHandler
        .AddEntry<GameInput.Button>("SH_GoForward")
        .CreateInput()
        .WithKeyboardBinding(GameInputHandler.Paths.Mouse.ForwardButton)
        .AvoidConflicts()
        .WithCategory(OtherCategory);

    private class EnglishTranslationAttribute : Attribute
    {
        public string Translation { get; }

        public EnglishTranslationAttribute(string translation)
        {
            Translation = translation;
        }
    }
}