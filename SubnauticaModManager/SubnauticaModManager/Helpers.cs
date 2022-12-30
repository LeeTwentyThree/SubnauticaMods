namespace SubnauticaModManager;

internal static class Helpers
{
    public static void FixText(TextMeshProUGUI text, FontManager.FontType font = FontManager.FontType.Aller_Rg)
    {
        text.font = FontManager.GetFontAsset(font);
    }

    public static void FixButton(Button button, bool colorSwap = true)
    {
        if (colorSwap) button.gameObject.EnsureComponent<uGUI_BasicColorSwap>();
    }

    public static void FixUIObjects(GameObject root)
    {
        var texts = root.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (var text in texts)
            FixText(text);
        var buttons = root.GetComponentsInChildren<Button>(true);
        foreach (var button in buttons)
            FixButton(button);
    }

    public static FMODAsset GetFmodAsset(string path)
    {
        var asset = ScriptableObject.CreateInstance<FMODAsset>();
        asset.path = path;
        asset.id = path;
        return asset;
    }
}
/// <summary>
/// Enum primarily used in the <see cref="ExtensionMethods.Compare"/> method.
/// </summary>
public enum CompareMode
{
    /// <summary>
    /// a == a, A == a, b != a
    /// </summary>
    Equals,

    /// <summary>
    /// A != a
    /// </summary>
    EqualsCaseSensitive,

    /// <summary>
    /// Whether this string starts with the given string. Not case sensitive.
    /// </summary>
    StartsWith,

    /// <summary>
    /// Whether this string starts with the given string. Case sensitive.
    /// </summary>
    StartsWithCaseSensitive,

    /// <summary>
    /// Whether this string contains the given string. Not case sensitive.
    /// </summary>
    Contains,

    /// <summary>
    /// Whether this string contains the given string. Case sensitive.
    /// </summary>
    ContainsCaseSensitive,

    /// <summary>
    /// Whether this string ends with or starts with the given string. Not case sensitive.
    /// </summary>
    StartsWithOrEndsWith,

    /// <summary>
    /// Whether this string ends with or starts with the given string. Case sensitive.
    /// </summary>
    StartsWithOrEndsWithCaseSensitive,
}