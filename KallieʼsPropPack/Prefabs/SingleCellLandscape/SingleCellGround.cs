using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using UnityEngine;

namespace KallieʼsPropPack.Prefabs.SingleCellLandscape;

public class SingleCellGround
{
    private const string ObstructionRockClassId = "56531208-8d32-47e9-8207-52acbfb7d8ff";
    
    private PrefabInfo Info { get; }
    private LargeWorldEntity.CellLevel CellLevel { get; }
    private bool BaseIsRock { get; }

    public SingleCellGround(string classId, LargeWorldEntity.CellLevel cellLevel, bool baseIsRock)
    {
        Info = PrefabInfo.WithTechType(classId);
        CellLevel = cellLevel;
        BaseIsRock = baseIsRock;
    }

    public void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(new CloneTemplate(Info, ObstructionRockClassId)
        {
            ModifyPrefab = ModifyPrefab
        });
        prefab.Register();
    }

    private void ModifyPrefab(GameObject prefab)
    {
        var largeWorldEntity = prefab.GetComponent<LargeWorldEntity>();
        largeWorldEntity.cellLevel = CellLevel;
        var renderer = prefab.GetComponentInChildren<Renderer>();
        var material = renderer.material;
        material.color = BaseIsRock ? new Color(0.3f, 0.3f, 0.3f) : Color.black;
        material.SetColor("_CapColor", Color.black);
        material.SetFloat("_CapBorderBlendRange", 0.13f);
        material.SetFloat("_CapBorderBlendOffset", 0.4f);
        material.SetFloat("_CapBorderBlendAngle", 1.88f);
        material.SetFloat("_Gloss", 0.49f);
        material.SetColor("_SpecColor", new Color(3, 3, 3));
        material.SetColor("_CapSpecColor", new Color(3, 3, 3));
        material.SetFloat("_CapScale", 0.04f);
    }
}