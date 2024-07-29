using Nautilus.Assets;
using Nautilus.Utility;
using UnityEngine;

namespace TestingZone;

public static class GlossTest
{
    public static void Register()
    {
        var prefab = new CustomPrefab(PrefabInfo.WithTechType("GlossTest"));
        var obj = Plugin.Bundle.LoadAsset<GameObject>("TestPlane");
        PrefabUtils.AddBasicComponents(obj, prefab.Info.ClassID, prefab.Info.TechType, LargeWorldEntity.CellLevel.Medium);
        MaterialUtils.ApplySNShaders(obj, 8f, 1f);
        var renderer = obj.GetComponent<Renderer>();
        var material = renderer.material;
        material.SetTexture("_MultiColorMask", Plugin.Bundle.LoadAsset<Texture2D>("TestColorMask"));
        prefab.SetGameObject(obj);
        prefab.Register();
    }
}