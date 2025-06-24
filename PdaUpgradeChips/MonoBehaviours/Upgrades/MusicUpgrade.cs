using PdaUpgradeChips.Data;
using PdaUpgradeChips.MonoBehaviours.UI;

namespace PdaUpgradeChips.MonoBehaviours.Upgrades;

public class MusicUpgrade : UpgradeChipBase
{
    private void Start()
    {
        PdaElements.MusicPlayerPanel.SetElementActive(true);
    }

    private void OnDestroy()
    {
        PdaElements.MusicPlayerPanel.SetElementActive(false);

        var musicPlayer = MusicPlayerUI.Main;
        if (musicPlayer)
            musicPlayer.OnMusicCardDestroyed();
    }
}