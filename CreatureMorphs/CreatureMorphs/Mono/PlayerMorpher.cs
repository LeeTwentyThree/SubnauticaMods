using UnityEngine.UI;
using UnityEngine.Events;
using UWE;
using CreatureMorphs.Mono.UI;

namespace CreatureMorphs.Mono;

internal class PlayerMorpher : MonoBehaviour
{
    public static PlayerMorpher main;

    private Player _player;
    private bool _morphing;
    private PossessedCreature _currentMorph;

    private GameObject _playerModelRoot;

    public bool CanMorph { get { return !_morphing && _currentMorph == null; } }

    public static bool PlayerCurrentlyMorphed
    {
        get
        {
            if (main == null) return false;
            return main.GetCurrentMorph() != null;
        }
    }

    private void Awake()
    {
        main = this;
        _player = GetComponent<Player>();
        _playerModelRoot = _player.gameObject.transform.Find("body/player_view/male_geo").gameObject;
        DevConsole.RegisterConsoleCommand(this, "morph", false, false);
    }

    private void OnConsoleCommand_morph(NotificationCenter.Notification n)
    {
        ErrorMessage.AddMessage(MorphCommand((string)n.data[0]));
    }

    private string MorphCommand(string techTypeName)
    {
        if (!TechTypeExtensions.FromString(techTypeName, out var techType, true)) return $"TechType '{techTypeName}' not found!";
        if (main == null) return "No instance of the PlayerMorphController was found!";
        if (!main.CanMorph) return "Player is unable to morph at this moment!";
        if (main.InitiateMorph(MorphDatabase.GetMorphType(techType)))
        {
            return $"Successfully morphing into '{techType}'";
        }
        else
        {
            return $"Failed to morph into '{techType}'";
        }
    }

    public bool InitiateMorph(MorphType morph)
    {
        if (!CanMorph)
        {
            ErrorMessage.AddMessage("Can't morph right now!");
            return false;
        }
        if (morph == null)
        {
            ErrorMessage.AddMessage("Invalid morph!");
            return false;
        }
        if (!SpaceToTransform(morph.sphereCheckRadius))
        {
            ErrorMessage.AddMessage("Not enough space to morph!");
            return false;
        }
        StartCoroutine(InitiateMorphCoroutine(morph));
        return true;
    }

    public void BecomeHuman()
    {
        if (_currentMorph != null)
        {
            var lm = _currentMorph.liveMixin;
            if (lm)
            {
                Player.main.liveMixin.health = Player.main.liveMixin.maxHealth * (lm.health / lm.maxHealth);
            }
            Player.main.SetPosition(_currentMorph.transform.position);
            Destroy(_currentMorph.gameObject);
        }
        TogglePlayerModel(true);
    }

    private void TogglePlayerModel(bool enabled)
    {
        foreach (var r in _playerModelRoot.GetComponentsInChildren<Renderer>(true))
        {
            r.enabled = enabled;
        }
        _player.liveMixin.invincible = !enabled;
        _player.rigidBody.isKinematic = !enabled;
        if (enabled) _player.UnfreezeStats();
        else _player.FreezeStats();
        AvatarInputHandler.main.gameObject.SetActive(enabled);
        if (enabled) GameModeUtils.DeactivateCheat(GameModeOption.NoAggression);
        else GameModeUtils.ActivateCheat(GameModeOption.NoAggression);
    }

    public PossessedCreature GetCurrentMorph()
    {
        return _currentMorph;
    }

    private bool SpaceToTransform(float radius)
    {
        return UWE.Utils.OverlapSphereIntoSharedBuffer(Player.main.transform.position, radius, LayerID.TerrainCollider, QueryTriggerInteraction.Ignore) == 0;
    }

    private IEnumerator InitiateMorphCoroutine(MorphType morph)
    {
        _morphing = true;
        float timeStarted = Time.time;
        if (MorphMenu.main != null) Destroy(MorphMenu.main.gameObject);
        var r = PrefabDatabase.GetPrefabAsync(morph.MorphClassId);
        yield return r;
        r.TryGetPrefab(out var prefab);
        var spawnedCreature = Instantiate(prefab, Helpers.CameraTransform.position, Helpers.CameraTransform.rotation);
        spawnedCreature.SetActive(false);
        TogglePlayerModel(false);
        _currentMorph = PossessedCreature.ControlCreature(spawnedCreature, morph);
        var morphMode = MorphAnimationData.GetData(morph.morphModeType);
        FadingOverlay.PlayFX(Color.black, 0f, morphMode.transformationDuration, 1f);
        Utils.PlayFMODAsset(morphMode.soundAsset, Helpers.CameraTransform.position);
        yield return new WaitForSeconds(morphMode.transformationDuration);
        spawnedCreature.SetActive(true);
        _currentMorph.GainControl();
        _morphing = false;
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Cursor.lockState == CursorLockMode.None)
            return;

        // Morph key:
        if (Input.GetKeyDown(Plugin.Config.MorphKey))
        {
            if (_currentMorph == null)
            {
                if (MorphMenu.main == null)
                {
                    MorphMenu.CreateInstance();
                }
                else
                {
                    Destroy(MorphMenu.main.gameObject);
                }
            }
            else
            {
                BecomeHuman();
            }
        }
    }

    private void ChooseMorph(TechType techType)
    {
        InitiateMorph(MorphDatabase.GetMorphType(techType));
    }
}