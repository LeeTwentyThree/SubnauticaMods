using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using UnityEngine;

namespace KallieʼsPropPack.Prefabs.SingleCellLandscape;

public class SingleCellRib
{
    private PrefabInfo Info { get; }
    
    private string OriginalClassId { get; }

    public SingleCellRib(string classId, string originalClassId)
    {
        Info = PrefabInfo.WithTechType(classId);
        OriginalClassId = originalClassId;
    }

    public void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(new CloneTemplate(Info, OriginalClassId)
        {
            ModifyPrefab = ModifyPrefab
        });
        prefab.Register();
    }

    private void ModifyPrefab(GameObject prefab)
    {
        var material = prefab.GetComponentInChildren<Renderer>().material;
        material.color = Color.black;
        material.SetColor("_SpecColor", new Color(0.1f, 0.2f, 0.4f));
        material.SetFloat("_SpecInt", 2);
        material.SetFloat("_Shininess", 7);
        material.SetFloat("_Fresnel", 0.5f);
        material.SetTexture("_BumpMap", null);
        material.SetVector("_Scale", new Vector4(0.3f, 0.3f, 0.3f, 0.3f));
        material.SetVector("_Frequency", new Vector4(0.38f, 0.4f, 0.13f, 0.37f));
        material.SetVector("_Speed", new Vector4(0.62f, 0.35f, 0));
        material.SetFloat("_WaveUpMin", 1f);
        material.EnableKeyword("UWE_WAVING");
        material.DisableKeyword("UWE_DETAILMAP");
    }
}