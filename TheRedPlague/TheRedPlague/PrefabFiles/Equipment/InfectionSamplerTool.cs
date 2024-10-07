using System.Collections;
using Nautilus.Assets;
using UnityEngine;

namespace TheRedPlague.PrefabFiles.Equipment;

public class InfectionSamplerTool
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("InfectionSampler");

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.Register();
    }

    public static IEnumerator CreatePrefab(IOut<GameObject> prefab)
    {
        var obj = Object.Instantiate()
    }
}