namespace PdaUpgradeChips.MonoBehaviours.Upgrades;

public class TestUpgrade : UpgradeChipBase
{
    private void Start()
    {
        ErrorMessage.AddMessage("Added upgrade!");
    }

    private void OnDestroy()
    {
        ErrorMessage.AddMessage("Removed upgrade!");
    }

    private void Update()
    {
        ErrorMessage.AddMessage("active!");
    }
}