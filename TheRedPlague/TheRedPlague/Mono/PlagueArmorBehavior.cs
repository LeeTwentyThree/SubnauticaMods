using Story;
using TheRedPlague.PrefabFiles;
using UnityEngine;

namespace TheRedPlague.Mono;

// This class manages the plague armor model & the infection damage in the dunes
public class PlagueArmorBehavior : MonoBehaviour
{
    private Transform _parentBone;
    private GameObject _currentArmor;
    private float _timeTryDamageAgain;

    private bool _equipped;
    private bool _infectionDamageDisabled;

    private bool _addedInfectionVisuals;

    private void Start()
    {
        _parentBone = transform.Find("body/player_view/export_skeleton/head_rig/neck/chest/spine_3/spine_2");
    }

    private void Update()
    {
        if (Time.time > _timeTryDamageAgain)
        {
            if (!_equipped && WaterBiomeManager.main.GetBiome(Player.main.transform.position) == "dunes")
            {
                if (_infectionDamageDisabled)
                {
                    return;
                }

                if (StoryGoalManager.main.IsGoalComplete(StoryUtils.EnzymeRainEnabled.key))
                {
                    _infectionDamageDisabled = true;
                    return;
                }
                
                Player.main.liveMixin.TakeDamage(1, MainCamera.camera.transform.position + Random.onUnitSphere);
                
                if (!_addedInfectionVisuals && Player.main.gameObject.GetComponent<InfectAnything>() == null)
                {
                    var infectVisuals = Player.main.gameObject.EnsureComponent<InfectAnything>();
                    infectVisuals.infectionHeightStrength = 0.01f;
                }

                _addedInfectionVisuals = true;
            }
            _timeTryDamageAgain = Time.time + 1f;
        }
    }

    public void SetArmorActive(bool state)
    {
        if (state && _currentArmor == null)
        {
            SpawnArmor();
        }
        else if (!state)
        {
            Destroy(_currentArmor);
        }

        _equipped = state;
    }

    private void SpawnArmor()
    {
        _currentArmor = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("BoneArmor_PlayerModel"), _parentBone, true);
        _currentArmor.transform.localPosition = new Vector3(-0.06f, 0, 0.03f);
        _currentArmor.transform.localEulerAngles = new Vector3(0, 10, 86);
        _currentArmor.transform.localScale = Vector3.one * 0.8f;
        var material = BoneArmor.GetMaterial();

        var renderers = _currentArmor.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.material = material;
        }

        _currentArmor.EnsureComponent<SkyApplier>().renderers = renderers;
    }
}