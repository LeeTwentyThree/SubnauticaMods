using System.Collections;
using System.Collections.Generic;
using DeExtinction.Mono;
using ECCLibrary;
using ECCLibrary.Data;
using ECCLibrary.Mono;
using Nautilus.Assets;
using Nautilus.Utility;
using UnityEngine;

namespace DeExtinction.Prefabs.Creatures;

internal class GulperLeviathanPrefab : CreatureAsset
{
    public GulperLeviathanPrefab(PrefabInfo prefabInfo) : base(prefabInfo)
    {
        CreatureDataUtils.AddCreaturePDAEncyclopediaEntry(this, "Lifeforms/Fauna/Leviathans", null, null, 8,
            Plugin.AssetBundle.LoadAsset<Texture2D>("Gulper_Ency"),
            Plugin.AssetBundle.LoadAsset<Sprite>("Gulper_Popup"));
    }

    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(Plugin.AssetBundle.LoadAsset<GameObject>("Gulper_Prefab"),
            BehaviourType.Leviathan, EcoTargetType.Leviathan, 5000)
        {
            SwimRandomData = new SwimRandomData(0.1f, 12f, new Vector3(40, 6, 40), 4f, 1f, true),
            AvoidTerrainData = new AvoidTerrainData(0.9f, 14f, 30f, 35f, 0.5f, 10),
            StayAtLeashData = new StayAtLeashData(0.4f, 12, 60),
            LocomotionData = new LocomotionData(12f, 0.4f, 3, 0.6f),
            Mass = 2000,
            CellLevel = LargeWorldEntity.CellLevel.Far,
            AnimateByVelocityData = new AnimateByVelocityData(24),
            AttackLastTargetData = new AttackLastTargetData(0.8f, 20f, 0.6f, 10f, 10f),
            AggressiveToPilotingVehicleData = new AggressiveToPilotingVehicleData(30f, 0.2f),
            EyeFOV = -0.5f,
            BehaviourLODData = new BehaviourLODData(40, 100, 200),
            ScannerRoomScannable = true,
            RespawnData = new RespawnData(false),
            AggressiveWhenSeeTargetList = new List<AggressiveWhenSeeTargetData>()
            {
                new(EcoTargetType.Shark, 1.5f, 80f, 3),
                new(EcoTargetType.SubDecoy, 2f, 35f, 2),
                new(EcoTargetType.MediumFish, 1.5f, 15f, 2)
            },
            CanBeInfected = false
        };
        
        return template;
    }

    protected override void ApplyMaterials(GameObject prefab)
    {
        MaterialUtils.ApplySNShaders(prefab, 8, 3, 3);
    }

    protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
    {
        var spine2 = prefab.SearchChild("Spine2").transform;
        var trailManagerBuilder = new TrailManagerBuilder(components, spine2, 3)
        {
            Trails = new Transform[]
            {
                spine2.SearchChild("Spine3").transform, spine2.SearchChild("Spine4").transform,
                spine2.SearchChild("Spine5").transform, spine2.SearchChild("Spine6").transform,
                spine2.SearchChild("Spine7").transform, spine2.SearchChild("Spine8").transform,
                spine2.SearchChild("Spine9").transform
            }
        };
        trailManagerBuilder.Apply();
        
        var mouth = prefab.SearchChild("Mouth");
        var lClawTrigger = prefab.SearchChild("LClaw");
        var rClawTrigger = prefab.SearchChild("RClaw");

        var voice = prefab.AddComponent<CreatureVoice>();
        var emitter = prefab.AddComponent<FMOD_CustomEmitter>();
        emitter.followParent = true;
        voice.emitter = emitter;
        voice.minInterval = 15;
        voice.maxInterval = 22;
        voice.closeIdleSound = AudioUtils.GetFmodAsset("GulperRoar");
        voice.farIdleSound = AudioUtils.GetFmodAsset("GulperRoarFar");
        voice.farThreshold = 70;
        voice.playSoundOnStart = true;
        voice.animator = components.Animator;
        voice.animatorTriggerParam = "roar";
        
        var gulperBehaviour = prefab.AddComponent<GulperBehaviour>();
        gulperBehaviour.creature = components.Creature;

        var meleeAttack = CreaturePrefabUtils.AddMeleeAttack<GulperMeleeAttackMouth>(prefab, components, mouth, true, 100);
        meleeAttack.canBeFed = false;
        meleeAttack.eatHungerDecrement = 0.05f;
        meleeAttack.eatHappyIncrement = 0.1f;
        meleeAttack.biteAggressionDecrement = 0.02f;
        meleeAttack.biteAggressionThreshold = 0.1f;
        meleeAttack.lastTarget = components.LastTarget;
        meleeAttack.creature = components.Creature;
        meleeAttack.liveMixin = components.LiveMixin;
        meleeAttack.animator = components.Creature.GetAnimator();
        
        AddClawAttack(prefab, "LClaw", "swipeL", components);
        AddClawAttack(prefab, "RClaw", "swipeR", components);
        
        yield break;
    }
    
    private void AddClawAttack(GameObject prefab, string triggerName, string animationName, CreatureComponents components)
    {
        var clawObject = prefab.SearchChild(triggerName);
        var clawAttack = CreaturePrefabUtils.AddMeleeAttack<GulperMeleeAttackClaw>(prefab, components, clawObject, true, 40);
        clawAttack.canBeFed = false;
        clawAttack.eatHungerDecrement = 0.05f;
        clawAttack.eatHappyIncrement = 0.1f;
        clawAttack.biteAggressionDecrement = 0.02f;
        clawAttack.biteAggressionThreshold = 0.1f;
        clawAttack.lastTarget = components.LastTarget;
        clawAttack.creature = components.Creature;
        clawAttack.liveMixin = components.LiveMixin;
        clawAttack.animator = components.Creature.GetAnimator();
        clawAttack.animationTriggerName = animationName;
        clawAttack.clawObject = clawObject;
    }
}