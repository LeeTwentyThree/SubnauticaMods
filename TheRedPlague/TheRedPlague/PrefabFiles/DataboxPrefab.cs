using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using UnityEngine;

namespace TheRedPlague.PrefabFiles;

public class DataboxPrefab
{
    public PrefabInfo Info { get; }
    
    public TechType UnlockTechType { get; }

    public DataboxPrefab(PrefabInfo info, TechType unlockTechType)
    {
        Info = info;
        UnlockTechType = unlockTechType;
    }

    public void Register()
    {
        var prefab = new CustomPrefab(Info);
        var cloneTemplate = new CloneTemplate(Info, "8cae16e8-f362-4c07-9375-39df60e8ea87");
        cloneTemplate.ModifyPrefab += ModifyPrefab;
        prefab.SetGameObject(cloneTemplate);
        prefab.Register();
    }

    private void ModifyPrefab(GameObject prefab)
    {
        var blueprintHandTarget = prefab.GetComponent<BlueprintHandTarget>();
        blueprintHandTarget.unlockTechType = UnlockTechType;
    }
}