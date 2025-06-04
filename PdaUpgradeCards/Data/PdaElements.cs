using Nautilus.Extensions;
using Nautilus.Utility;
using PdaUpgradeCards.MonoBehaviours;
using PdaUpgradeCards.MonoBehaviours.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PdaUpgradeCards.Data;

public static class PdaElements
{
    public static CustomPdaButton OpenMenuButton { get; private set; }
    public static CustomPdaElement MusicPlayerPanel { get; private set; }

    public static void RegisterAll()
    {
        OpenMenuButton = new CustomPdaButton(
            "OpenUpgradesMenuButton",
            rect => { rect.anchoredPosition = new Vector2(500, 240); },
            new CustomPdaButton.ButtonIcons("ViewUpgradeChipsButtonIcon", "ViewUpgradeChipsButtonSelectedIcon",
                "ViewUpgradeChipsButtonPressedIcon"),
            PDATab.Inventory,
            () => PdaUpgradesManager.Main.DisplayMenu(),
            true);

        MusicPlayerPanel = new CustomPdaElement(
            "MusicPlayer",
            rect =>
            {
                rect.anchoredPosition = new Vector2(0, -345);
                var playerPanel =
                    Object.Instantiate(Plugin.Bundle.LoadAsset<GameObject>("MusicPlayerPanel"), rect, true);
                var playerRect = playerPanel.GetComponent<RectTransform>();
                playerRect.anchoredPosition = new Vector2(0, 0);
                playerRect.localScale = Vector3.one;
                playerRect.localRotation = Quaternion.identity;
                FontUtils.SetFontInChildren(rect.gameObject, FontUtils.Aller_Rg);

                var musicPlayer = rect.gameObject.AddComponent<MusicPlayerUI>();
                musicPlayer.currentMusicText = musicPlayer.transform.SearchChild("CurrentSongText")
                    .GetComponent<TextMeshProUGUI>();
                var progressBar = musicPlayer.transform.SearchChild("ProgressBar").GetComponent<Slider>();
                musicPlayer.progressBar = progressBar;
                progressBar.onValueChanged.AddListener(musicPlayer.OnProgressBarChanged);
                var volumeSlider = musicPlayer.transform.SearchChild("VolumeSlider").GetComponent<Slider>();
                musicPlayer.volumeSlider = volumeSlider;
                volumeSlider.onValueChanged.AddListener(musicPlayer.OnVolumeSliderChanged);
                musicPlayer.playButtonImage = musicPlayer.transform.SearchChild("PlayButton").GetComponent<Image>();
                musicPlayer.playSprite = Plugin.Bundle.LoadAsset<Sprite>("PlayButton");
                musicPlayer.pauseSprite = Plugin.Bundle.LoadAsset<Sprite>("PauseButton");
                musicPlayer.restartSprite = Plugin.Bundle.LoadAsset<Sprite>("RestartSongButton");

                musicPlayer.transform.SearchChild("PreviousButton").GetComponent<Button>().onClick
                    .AddListener(musicPlayer.OnPreviousButton);
                musicPlayer.transform.SearchChild("PlayButton").GetComponent<Button>().onClick
                    .AddListener(musicPlayer.OnPlayButton);
                musicPlayer.transform.SearchChild("NextButton").GetComponent<Button>().onClick
                    .AddListener(musicPlayer.OnNextButton);
                musicPlayer.transform.SearchChild("MusicFolderButton").GetComponent<Button>().onClick
                    .AddListener(musicPlayer.OnOpenFolderButton);
            },
            PDATab.Inventory,
            false);
    }
}