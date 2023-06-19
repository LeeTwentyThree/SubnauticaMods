namespace CreatureMorphs;

public class MorphMode
{
    public float transformationDuration;
    public FMODAsset soundAsset;

    public MorphMode(float transformationDuration, FMODAsset soundAsset)
    {
        this.transformationDuration = transformationDuration;
        this.soundAsset = soundAsset;
    }
}