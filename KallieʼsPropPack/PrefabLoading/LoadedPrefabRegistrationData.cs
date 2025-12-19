using System;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace KallieʼsPropPack.PrefabLoading;

[Serializable]
public class LoadedPrefabRegistrationData
{
    public string prefix;
    public string postfix;

    public PrefabFamily[] families;

    [Serializable]
    public class PrefabFamily
    {
        public string name;
        public string factoryClass;
        public string prefix;
        public string postfix;
        public VFXSurfaceTypes surfaceType;
        public LargeWorldEntity.CellLevel cellLevel;
        public bool constructionObstacle;
        public bool zUp;
        public MaterialSettings materialSettings;
        public PrefabVariant[] variants;
        public string[] prefabs;
    }

    [Serializable]
    public class PrefabVariant
    {
        public string postfix;
        public string childModelPath;
        public Parameter[] parameters;
    }

    [Serializable]
    public class MaterialSettings
    {
        public float shininess;
        public float specularIntensity;
        public float glowStrength;
    }

    [Serializable]
    public class Parameter
    {
        public string parameterName;
        public string parameterValue;
    }
}