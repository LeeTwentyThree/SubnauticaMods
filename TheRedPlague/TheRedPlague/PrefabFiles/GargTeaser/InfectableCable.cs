using System.Collections;
using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using UnityEngine;

namespace TheRedPlague.PrefabFiles.GargTeaser;

public class InfectableCable
{
    public PrefabInfo Info { get; }

    private readonly string _referenceClassId;

    public InfectableCable(string classId, string referenceClassId)
    {
        Info = PrefabInfo.WithTechType(classId);
        _referenceClassId = referenceClassId;
    }
    
    public void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(new CloneTemplate(Info, _referenceClassId)
        {
            ModifyPrefabAsync = ModifyPrefab
        });
        prefab.Register();
    }

    private IEnumerator ModifyPrefab(GameObject prefab)
    {
        prefab.AddComponent<Mono.PlagueGarg.InfectableCable>();
        yield break;
    }
}