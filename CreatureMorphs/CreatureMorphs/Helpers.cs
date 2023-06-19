namespace CreatureMorphs;

internal class Helpers
{
    public static Transform CameraTransform => MainCamera.camera.transform;

    public static FMODAsset GetFmodAsset(string audioPath)
    {
        FMODAsset asset = ScriptableObject.CreateInstance<FMODAsset>();
        asset.path = audioPath;
        return asset;
    }
}