using System.Collections;
using ECCLibrary;
using ECCLibrary.Data;
using Nautilus.Assets;
using TheRedPlague.Mono.CreatureBehaviour.Sucker;
using UnityEngine;

namespace TheRedPlague.PrefabFiles.Creatures;

public class SuckerController : CreatureAsset
{
    public SuckerController(PrefabInfo prefabInfo) : base(prefabInfo)
    {
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(Plugin.AssetBundle.LoadAsset<GameObject>("SuckerController"),
            BehaviourType.MediumFish, EcoTargetType.MediumFish, 100f)
        {
            SwimRandomData = new SwimRandomData(0.1f, 3f, new Vector3(10, 3, 10)),
            AnimateByVelocityData = new AnimateByVelocityData(4f),
            StayAtLeashData = new StayAtLeashData(0.3f, 3f, 20f),
            AggressiveToPilotingVehicleData = new AggressiveToPilotingVehicleData(25, 0.2f),
            AttackLastTargetData = new AttackLastTargetData(0.4f, 4f, 0.7f, 10f),
            BehaviourLODData = new BehaviourLODData(30, 50, 100),
            EyeFOV = -0.5f,
            Mass = 13,
            CellLevel = LargeWorldEntity.CellLevel.Medium,
            CanBeInfected = false
        };
        return template;
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        var grab = prefab.AddComponent<SuckerGrabVehicles>();
        
        grab.rigidbody = components.Rigidbody;
        grab.mainCollider = prefab.GetComponent<Collider>();
        
        prefab.transform.Find("sucker/SuckerEyePivot").gameObject.AddComponent<SuckerLook>();
        
        yield break;
    }

    protected override void PostRegister()
    {
        InfectionSettingsDatabase.InfectionSettingsList.Add(PrefabInfo.TechType, new InfectionSettings(Color.white, 0f));
    }
}