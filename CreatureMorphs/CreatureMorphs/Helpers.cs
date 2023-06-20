namespace CreatureMorphs;

internal static class Helpers
{
    public static Transform CameraTransform => MainCamera.camera.transform;

    public static FMODAsset GetFmodAsset(string audioPath)
    {
        FMODAsset asset = ScriptableObject.CreateInstance<FMODAsset>();
        asset.path = audioPath;
        return asset;
    }

    public static Transform SearchChild(this Transform transform, string name)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == name)
                return child;

            var recursive = SearchChild(child, name);
            if (recursive != null)
                return recursive;
        }
        return null;
    }

    public static string GetInputString(GameInput.Button button) => LanguageCache.GetButtonFormat("MorphInput", button);

    public static GameObject SearchChild(this GameObject gameObject, string name) => SearchChild(gameObject.transform, name).gameObject;
}