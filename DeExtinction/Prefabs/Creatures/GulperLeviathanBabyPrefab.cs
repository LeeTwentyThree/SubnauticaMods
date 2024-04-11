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

internal class GulperLeviathanBabyPrefab : CreatureAsset
{
    public PrefabInfo EggInfo { get; set; }

    public GulperLeviathanBabyPrefab(PrefabInfo prefabInfo) : base(prefabInfo)
    {
        CreatureDataUtils.AddCreaturePDAEncyclopediaEntry(this, "Lifeforms/Fauna/Leviathans", null, null, 3,
            Plugin.AssetBundle.LoadAsset<Texture2D>("GulperBaby_Ency"),
            Plugin.AssetBundle.LoadAsset<Sprite>("Gulper_Popup"));
    }
    
    protected override CreatureTemplate CreateTemplate()
    {
        var template = new CreatureTemplate(Plugin.AssetBundle.LoadAsset<GameObject>("GulperBaby_Prefab"),
            BehaviourType.Leviathan, EcoTargetType.Shark, 5000)
        {
            SwimRandomData = new SwimRandomData(0.1f, 6f, new Vector3(20, 7, 20), 1.5f, 0.5f, true),
            AvoidObstaclesData = new AvoidObstaclesData(0.4f, 6f, true, 9f, 10f),
            StayAtLeashData = new StayAtLeashData(0.4f, 6f, 30),
            LocomotionData = new LocomotionData(10f, 0.5f, 3, 0.6f),
            Mass = 300,
            CellLevel = LargeWorldEntity.CellLevel.Medium,
            AnimateByVelocityData = new AnimateByVelocityData(10),
            AttackLastTargetData = new AttackLastTargetData(0.8f, 12f, 0.6f, 10f, 10f),
            EyeFOV = -0.75f,
            BehaviourLODData = new BehaviourLODData(35, 60, 120),
            ScannerRoomScannable = true,
            RespawnData = new RespawnData(false),
            AggressiveWhenSeeTargetList = new List<AggressiveWhenSeeTargetData>()
            {
                new(EcoTargetType.Shark, 2, 40, 2),
                new(EcoTargetType.SubDecoy, 2, 35, 2),
                new(EcoTargetType.Leviathan, 1, 35, 1),
                new(EcoTargetType.SmallFish, 2, 30, 2)
            },
            BioReactorCharge = 800,
            PickupableFishData = new PickupableFishData(TechType.GarryFish, "Gulper Leviathan", "ViewModel")
        };
        
        template.SetWaterParkCreatureData(new WaterParkCreatureDataStruct(0.02f, 0.5f, 1f, 1.25f, true, true,
            EggInfo.ClassID));
        
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

        /* Mute the baby until it has proper sound files
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
        */
        
        var meleeAttack = CreaturePrefabUtils.AddMeleeAttack<GulperMeleeAttackMouth>(prefab, components, mouth, true, 50);
        meleeAttack.isBaby = true;
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
        var clawAttack = CreaturePrefabUtils.AddMeleeAttack<GulperMeleeAttackClaw>(prefab, components, clawObject, true, 20);
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
        clawAttack.isBaby = true;
    }
}