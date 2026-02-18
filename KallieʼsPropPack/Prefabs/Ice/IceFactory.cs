using System;
using System.Collections;
using KallieʼsPropPack.MaterialModifiers;
using KallieʼsPropPack.PrefabLoading;
using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;

namespace KallieʼsPropPack.Prefabs.Ice;

public class IceFactory : IEpicPrefabFactory
{
    public IEnumerator BuildVariant(GameObject prefab, LoadedPrefabRegistrationData.Parameter[] parameters)
    {
        var renderer = prefab.GetComponentInChildren<Renderer>();
        var material = renderer.material;
        bool isTransparent = false;
        bool isDark = false;
        foreach (var param in parameters)
        {
            if (param.parameterName.Equals("Transparent"))
            {
                isTransparent = param.parameterValue.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
            if (param.parameterName.Equals("Dark"))
            {
                isDark = param.parameterValue.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
        }

        var materialType = isTransparent ? MaterialUtils.MaterialType.Transparent : MaterialUtils.MaterialType.Opaque;
        MaterialUtils.ApplyUBERShader(material, 4, 50, 0.2f, materialType);

        var modifier = new IceMaterialModifier(isDark);
        modifier.EditMaterial(material, renderer, 0, materialType);
        
        yield break;
    }

    // UNUSED; materials are skipped!
    public MaterialModifier[] MaterialModifiers => null;
}