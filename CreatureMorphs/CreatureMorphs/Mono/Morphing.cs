using UnityEngine.UI;
using UnityEngine.Events;
using UWE;

namespace CreatureMorphs.Mono;
internal class Morphing : MonoBehaviour
{
    public static Morphing main;

    private Player _player;
    private bool _morphing;
    private MorphInstance _currentMorph;

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
        main.InitiateMorph(MorphDatabase.GetMorphType(techType));
        return $"Successfully morphing into '{techType}'";
    }

    public void InitiateMorph(MorphType morph)
    {
        if (!CanMorph) return;
        StartCoroutine(InitiateMorphCoroutine(morph));
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
        if (enabled) _player.FreezeStats();
        else _player.UnfreezeStats();
    }

    public MorphInstance GetCurrentMorph()
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
        _currentMorph = MorphInstance.ControlCreature(spawnedCreature, morph);
        var morphMode = MorphModeData.GetData(morph.morphModeType);
        FadingOverlay.PlayFX(Color.black, 0.1f, morphMode.transformationDuration, 1f);
        Utils.PlayFMODAsset(morphMode.soundAsset, Helpers.CameraTransform.position);
        yield return new WaitForSeconds(morphMode.transformationDuration);
        _currentMorph.GainControl();
        _morphing = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (_currentMorph == null)
            {
                // spawn menu!
            }
            else
            {
                BecomeHuman();
            }
        }
    }

    private Button.ButtonClickedEvent GetEvent(UnityAction call)
    {
        var ev = new Button.ButtonClickedEvent();
        ev.AddListener(call);
        return ev;
    }

    private void ChooseMorph(TechType techType)
    {
        InitiateMorph(MorphDatabase.GetMorphType(techType));
    }
}