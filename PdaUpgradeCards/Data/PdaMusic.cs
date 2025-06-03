namespace PdaUpgradeCards.Data;

public class PdaMusic
{
    public PdaMusic(string id, string eventPath)
    {
        Id = id;
        EventPath = eventPath;
    }

    public string Id { get; }
    public string EventPath { get; }
}