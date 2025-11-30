using Nautilus.Utility;
using UnityEngine;

namespace KallieʼsPropPack.Prefabs.Lab;

public class LabFloorPlate : LabPropBase
{
    public LabFloorPlate(string id) : base(id, id, LargeWorldEntity.CellLevel.Medium)
    {
    }

    protected override void ApplyMaterials(GameObject obj)
    {
        MaterialUtils.ApplySNShaders(obj, 6f, 1, 2.5f);
    }
}