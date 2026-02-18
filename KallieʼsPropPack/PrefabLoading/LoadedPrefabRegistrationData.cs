using System;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace KallieʼsPropPack.PrefabLoading;

[Serializable]
public class LoadedPrefabRegistrationData
{
    public string prefix;
    public string postfix;
    public string customFolderPath;

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
        public bool ignore;
    }

    [Serializable]
    public class Parameter
    {
        public string parameterName;
        public string parameterValue;

        public T GetValue<T>()
        {
            if (string.IsNullOrEmpty(parameterValue))
                return default;

            var type = typeof(T);

            if (type == typeof(bool))
                return (T)(object)bool.Parse(parameterValue);

            if (type == typeof(int))
                return (T)(object)int.Parse(parameterValue);

            if (type == typeof(float))
                return (T)(object)float.Parse(parameterValue);

            if (type == typeof(string))
                return (T)(object)parameterValue;

            throw new NotSupportedException($"Type {type} is not supported.");
        } 
    }
}