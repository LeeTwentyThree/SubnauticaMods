using Nautilus.Utility;
using UnityEngine;

namespace KallieʼsPropPack.Prefabs.Lab;

public class LabFlotationDevice : LabPropBase
{
    public LabFlotationDevice(string modelName) : base(modelName, modelName, LargeWorldEntity.CellLevel.Medium)
    {
    }

    protected override void ApplyMaterials(GameObject obj)
    {
        MaterialUtils.ApplySNShaders(obj, 6, 2);
    }
}