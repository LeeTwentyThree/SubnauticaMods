using System.Collections;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;

namespace KallieʼsPropPack.PrefabLoading;

public interface IEpicPrefabFactory
{
    IEnumerator BuildVariant(GameObject prefab, LoadedPrefabRegistrationData.Parameter[] parameters);
    MaterialModifier[] MaterialModifiers { get; }
}