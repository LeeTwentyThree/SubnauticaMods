namespace TheRedPlague.Mono.FleshBlobs;

public class FleshBlobSound
{
    public readonly FMODAsset asset;
    public readonly float minInterval;
    public readonly float maxInterval;
    public readonly float minDistance;
    public readonly float maxDistance;
    public readonly float minSize;
    
    public FleshBlobSound(FMODAsset asset, float minInterval, float maxInterval, float minDistance, float maxDistance, float minSize)
    {
        this.asset = asset;
        this.minInterval = minInterval;
        this.maxInterval = maxInterval;
        this.minDistance = minDistance;
        this.maxDistance = maxDistance;
        this.minSize = minSize;
    }
}