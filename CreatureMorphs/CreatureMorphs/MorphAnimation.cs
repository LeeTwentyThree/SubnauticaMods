namespace CreatureMorphs;

internal class MorphAnimation
{
    public float transformationDuration;
    public FMODAsset soundAsset;

    public MorphAnimation(float transformationDuration, FMODAsset soundAsset)
    {
        this.transformationDuration = transformationDuration;
        this.soundAsset = soundAsset;
    }
}