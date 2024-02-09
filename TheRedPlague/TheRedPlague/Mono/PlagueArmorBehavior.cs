using TheRedPlague.PrefabFiles;
using UnityEngine;

namespace TheRedPlague.Mono;

public class PlagueArmorBehavior : MonoBehaviour
{
    private Transform _parentBone;
    private GameObject _currentArmor;

    private void Start()
    {
        _parentBone = transform.Find("body/player_view/export_skeleton/head_rig/neck/chest/spine_3/spine_2");
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
    }

    private void SpawnArmor()
    {
        _currentArmor = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("BoneArmor_PlayerModel"), _parentBone, true);
        _currentArmor.transform.localPosition = new Vector3(0, 0, 0.03f);
        _currentArmor.transform.localEulerAngles = new Vector3(0, 10, 86);
        _currentArmor.transform.localScale = Vector3.one * 0.9f;
        var material = BoneArmor.GetMaterial();

        var renderers = _currentArmor.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.material = material;
        }
        
        _currentArmor.AddComponent<SkyApplier>();
    }
}