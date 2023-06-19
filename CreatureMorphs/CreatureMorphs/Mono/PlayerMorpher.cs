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
        if (!CanMorph) return false;
        if (morph == null) return false;
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
    }

    public PossessedCreature GetCurrentMorph()
    {
        return _currentMorph;
    }

    private IEnumerator InitiateMorphCoroutine(MorphType morph)
    {
        _morphing = true;
        var r = PrefabDatabase.GetPrefabAsync(morph.MorphClassId);
        yield return r;
        r.TryGetPrefab(out var prefab);
        var spawnedCreature = Instantiate(prefab, Helpers.CameraTransform.position, Helpers.CameraTransform.rotation);
        spawnedCreature.SetActive(true);
        TogglePlayerModel(false);
        _currentMorph = PossessedCreature.ControlCreature(spawnedCreature, morph);
        var morphMode = MorphModeData.GetData(morph.morphModeType);
        FadingOverlay.PlayFX(Color.black, 0.1f, morphMode.transformationDuration, 1f);
        Utils.PlayFMODAsset(morphMode.soundAsset, Helpers.CameraTransform.position);
        yield return new WaitForSeconds(morphMode.transformationDuration);
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