using System.Collections;
using Nautilus.Assets;
using TheRedPlague.Mono;
using TheRedPlague.Mono.CreatureBehaviour.Sucker;
using UnityEngine;

namespace TheRedPlague.PrefabFiles;

public class PossessedVehicle
{
    public PrefabInfo Info { get; }
    
    private readonly TechType _vehicleTechType;

    public PossessedVehicle(TechType vehicleTechType)
    {
        Info = PrefabInfo.WithTechType("Possessed" + vehicleTechType);
        _vehicleTechType = vehicleTechType;
    }

    public void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(BuildPrefab);
        prefab.Register();
    }

    private IEnumerator BuildPrefab(IOut<GameObject> prefab)
    {
        var task = CraftData.GetPrefabForTechTypeAsync(_vehicleTechType);
        yield return task;
        var obj = UWE.Utils.InstantiateDeactivated(task.GetResult());
        
        // lock vehicle
        var vehicle = obj.GetComponent<Vehicle>();
        var infectedVehicle = obj.AddComponent<InfectedVehicle>();
        obj.AddComponent<SuckerControllerTarget>();
        
        // infect vehicle
        var infect = obj.AddComponent<InfectAnything>();
        infect.infectionAmount = 1;
        
        // remove signal/beacon
        Object.DestroyImmediate(obj.GetComponent<PingInstance>());
        
        // remove construct vfx
        Object.DestroyImmediate(obj.GetComponent<VFXConstructing>());
        
        // fix rigidity
        obj.GetComponent<Rigidbody>().isKinematic = false;
        
        prefab.Set(obj);
    }
}