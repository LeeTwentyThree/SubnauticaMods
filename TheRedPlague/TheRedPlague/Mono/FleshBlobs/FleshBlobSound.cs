namespace TheRedPlague.Mono.FleshBlobs;

public class FleshBlobSound
{
    public FMODAsset asset;
    public float minInterval;
    public float maxInterval;
    public float minDistance;
    public float maxDistance;
    
    public FleshBlobSound(FMODAsset asset, float minInterval, float maxInterval, float minDistance, float maxDistance)
    {
        this.asset = asset;
        this.minInterval = minInterval;
        this.maxInterval = maxInterval;
        this.minDistance = minDistance;
        this.maxDistance = maxDistance;
    }
}