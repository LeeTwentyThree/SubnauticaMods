using Story;
using UnityEngine;

namespace TheRedPlague.Mono;

public class DisableDomeTerminal : HandTarget, IHandTarget, IStoryGoalListener
{
    public FMODAsset accessGrantedSound;

    public FMODAsset accessDeniedSound;

    public PlayerCinematicController cinematic;

    public FMODAsset useSound;

    public FMODAsset curedUseSound;

    public FMOD_CustomLoopingEmitter openLoopSound;

    public StoryGoal onPlayerCuredGoal;

    public ParticleSystem glowRing;

    public Material glowMaterial;

    private Player _usingPlayer;

    private bool _opened;

    private bool _ignorePlayer;

    private bool _triggeredDeniedStory;

    private bool _playerCured;

    private void Start()
    {
        StoryGoalManager main = StoryGoalManager.main;
        if (main)
        {
            _playerCured = main.IsGoalComplete(onPlayerCuredGoal.key);
            if (!_playerCured)
            {
                main.AddListener(this);
            }
        }
    }

    public void NotifyGoalComplete(string key)
    {
        if (string.Equals(key, onPlayerCuredGoal.key, System.StringComparison.OrdinalIgnoreCase))
        {
            _playerCured = true;
        }
    }

    private void SetOpen(bool isOpen)
    {
        if (isOpen)
        {
            openLoopSound.Play();
        }
        else
        {
            openLoopSound.Stop();
        }

        _opened = isOpen;
        cinematic.animator.SetBool("open", isOpen);
    }

    public void OnHandHover(GUIHand hand)
    {
        if (!StoryGoalManager.main.IsGoalComplete(StoryUtils.DisableDome.key) && !(_usingPlayer != null) && _opened)
        {
            HandReticle.main.SetText(HandReticle.TextType.Hand, "DisableDomePrompt", translate: true,
                GameInput.Button.LeftHand);
            HandReticle.main.SetText(HandReticle.TextType.HandSubscript, string.Empty, translate: false);
            HandReticle.main.SetIcon(HandReticle.IconType.Interact);
        }
    }

    public void OnHandClick(GUIHand hand)
    {
        if (_usingPlayer == null && PlayerCinematicController.cinematicModeCount <= 0 && _opened)
        {
            _usingPlayer = hand.player;
            Inventory.main.ReturnHeld();
            if (_playerCured)
            {
                Utils.PlayFMODAsset(curedUseSound, transform);
            }
            else
            {
                Utils.PlayFMODAsset(useSound, transform);
            }

            _usingPlayer.playerAnimator.SetBool("using_tool_first", false);
            _usingPlayer.playerAnimator.SetBool("cured", _playerCured);
            cinematic.animator.SetBool("first_use", false);
            cinematic.animator.SetBool("cured", _playerCured);
            cinematic.StartCinematicMode(hand.player);
            Invoke(_playerCured ? "SetLightAccessGranted" : "SetLightAccessDenied", 5.75f);
        }
    }

    private void SetLightAccessGranted()
    {
        glowMaterial.SetColor(ShaderPropertyID._Color, Color.green);
        glowRing.Play();
    }

    private void SetLightAccessDenied()
    {
        glowMaterial.SetColor(ShaderPropertyID._Color, Color.red);
        glowRing.Play();
    }

    public void OnPlayerCinematicModeEnd()
    {
        if ((bool) _usingPlayer)
        {
            if (!_playerCured)
            {
                Utils.PlayFMODAsset(accessDeniedSound, base.transform);
                // disableDeniedGoal.Trigger();
                if (!_triggeredDeniedStory)
                {
                    // lostRiverHintGoal.Trigger();
                    _triggeredDeniedStory = true;
                }
            }
            else
            {
                Utils.PlayFMODAsset(accessGrantedSound, transform);
                StoryUtils.DisableDomeEvent();
            }

            SetOpen(isOpen: false);
            _ignorePlayer = true;
            _usingPlayer.playerAnimator.SetBool("using_tool_first", value: false);
        }

        _usingPlayer = null;
    }

    public void OnTerminalAreaEnter()
    {
        if (_ignorePlayer)
        {
            _ignorePlayer = false;
        }
        else
        {
            SetOpen(isOpen: true);
        }
    }

    public void OnTerminalAreaExit()
    {
        SetOpen(isOpen: false);
    }
}