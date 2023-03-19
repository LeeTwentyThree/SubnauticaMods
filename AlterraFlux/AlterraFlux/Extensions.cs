namespace AlterraFlux;

public static class Extensions
{
    public static GameObject SearchChild(this GameObject gameObject, string byName, CompareMode stringComparison = CompareMode.EqualsCaseSensitive)
    {
        GameObject obj = SearchChildRecursive(gameObject, byName, stringComparison);
        return obj;
    }

    private static GameObject SearchChildRecursive(GameObject gameObject, string byName, CompareMode stringComparison)
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.name.Compare(byName, stringComparison))
            {
                return child.gameObject;
            }
            GameObject recursive = SearchChildRecursive(child.gameObject, byName, stringComparison);
            if (recursive)
            {
                return recursive;
            }
        }
        return null;
    }

    public static bool Compare(this string original, string compareTo, CompareMode comparisonMode)
    {
        switch (comparisonMode)
        {
            default:
                return original == compareTo;
            case CompareMode.Equals:
                return original.ToLower() == compareTo.ToLower();
            case CompareMode.EqualsCaseSensitive:
                return original == compareTo;
            case CompareMode.StartsWith:
                return original.ToLower().StartsWith(compareTo.ToLower());
            case CompareMode.StartsWithCaseSensitive:
                return original.StartsWith(compareTo);
            case CompareMode.Contains:
                return original.ToLower().Contains(compareTo.ToLower());
            case CompareMode.ContainsCaseSensitive:
                return original.Contains(compareTo);
            case CompareMode.StartsWithOrEndsWith:
                return original.ToLower().StartsWith(compareTo.ToLower()) || original.ToLower().EndsWith(compareTo.ToLower());
            case CompareMode.StartsWithOrEndsWithCaseSensitive:
                return original.StartsWith(compareTo) || original.EndsWith(compareTo);
        }
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