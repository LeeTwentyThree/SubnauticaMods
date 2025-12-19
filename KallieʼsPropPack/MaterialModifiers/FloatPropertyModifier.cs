using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;

namespace KallieʼsPropPack.MaterialModifiers;

public class FloatPropertyModifier : MaterialModifier
{
    private readonly int _id;
    private readonly float _value;

    public FloatPropertyModifier(string propertyName, float value) : this(Shader.PropertyToID(propertyName), value)
    {
    }

    public FloatPropertyModifier(int id, float value)
    {
        _id = id;
        _value = value;
    }

    public override void EditMaterial(Material material, Renderer renderer, int materialIndex,
        MaterialUtils.MaterialType materialType)
    {
        material.SetFloat(_id, _value);
    }
}