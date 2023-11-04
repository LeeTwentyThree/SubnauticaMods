using Nautilus.Assets;
using Nautilus.Utility;
using System.Collections;
using TheRumbling.MaterialModifiers;
using TheRumbling.Mono;
using UnityEngine;

namespace TheRumbling.Prefabs;

internal static class FoundingTitanPrefab
{
    private static float lilly;

    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("FoundingTitan", "FOUND TITAN", "AOT spoilers lol");

    private static GameObject _smokeVfxOriginal;

    public static void Register()
    {
        var customPrefab = new CustomPrefab(Info);

        customPrefab.SetGameObject(GetGameObject);
        customPrefab.Register();
    }

    private static IEnumerator GetGameObject(IOut<GameObject> result)
    {
        var titan = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("FoundingTitan"));
        titan.gameObject.SetActive(false);
        PrefabUtils.AddBasicComponents(titan, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Global);
        MaterialUtils.ApplySNShaders(titan, 4, 1, 1, new TitanMaterialModifier(), new SteamMaterialModifier());
        var heightmapEntity = titan.AddComponent<HeightmapEntity>();
        heightmapEntity.SetEssentials(Balance.DefaultTitanWalkSpeed, -9.8f, 9.8f, -20f, -110, -110, 0);
        
        if (_smokeVfxOriginal == null)
        {
            _smokeVfxOriginal = CrashedShipExploder.main.transform
                .Find("unexplodedFX/Ship_Exterior_CrashedFX(Clone)/xFireHuge1").gameObject;
        }

        var smokeVfx = Object.Instantiate(_smokeVfxOriginal, titan.transform, false);
        smokeVfx.transform.localPosition = Vector3.zero;
        smokeVfx.transform.localScale = Vector3.one * 5f;
        smokeVfx.GetComponent<Renderer>().enabled = false;
        smokeVfx.transform.GetChild(0).gameObject.SetActive(false);
        smokeVfx.transform.GetChild(2).gameObject.SetActive(false);
        smokeVfx.transform.GetChild(4).gameObject.SetActive(false);

        result.Set(titan);
        yield break;
    }
}