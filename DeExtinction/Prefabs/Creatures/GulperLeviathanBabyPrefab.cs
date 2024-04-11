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
                new(EcoTargetType.Shark, 1.5f, 80f, 3),
                new(EcoTargetType.SubDecoy, 2f, 35f, 2),
                new(EcoTargetType.MediumFish, 1.5f, 15f, 2)
            },
            BioReactorCharge = 800
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

        /*
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
/*
public override BehaviourType BehaviourType => BehaviourType.Leviathan;

        public override WaterParkCreatureParameters WaterParkParameters => new WaterParkCreatureParameters(0.02f, 0.5f, 1f, 1.25f, false);

        public override Vector2int SizeInInventory => new Vector2int(4, 4);

        public override void AddCustomBehaviour(CreatureComponents components)
        {
            GameObject spine2 = prefab.SearchChild("Spine2");
            CreateTrail(spine2, new Transform[] { spine2.SearchChild("Spine3").transform, spine2.SearchChild("Spine4").transform, spine2.SearchChild("Spine5").transform, spine2.SearchChild("Spine6").transform, spine2.SearchChild("Spine7").transform, spine2.SearchChild("Spine8").transform, spine2.SearchChild("Spine9").transform }, components, 3f);
            MakeAggressiveTo(30f, 2, EcoTargetType.SmallFish, 0.2f, 1f);;
            MakeAggressiveTo(40f, 2, EcoTargetType.Shark, 0f, 1f).ignoreSameKind = false;
            MakeAggressiveTo(35f, 2, EcoTargetType.SubDecoy, 0f, 2f);
            MakeAggressiveTo(35f, 1, EcoTargetType.Leviathan, 0f, 1f);
            GameObject mouth = prefab.SearchChild("Mouth");
            GameObject lClawTrigger = prefab.SearchChild("LClaw");
            GameObject rClawTrigger = prefab.SearchChild("RClaw");

            GulperMeleeAttack_Mouth meleeAttack = prefab.AddComponent<GulperMeleeAttack_Mouth>();
            meleeAttack.isBaby = true;
            meleeAttack.mouth = mouth;
            meleeAttack.canBeFed = false;
            meleeAttack.biteInterval = 1f;
            meleeAttack.biteDamage = 50f;
            meleeAttack.eatHungerDecrement = 0.05f;
            meleeAttack.eatHappyIncrement = 0.1f;
            meleeAttack.biteAggressionDecrement = 0.02f;
            meleeAttack.biteAggressionThreshold = 0.1f;
            meleeAttack.lastTarget = components.lastTarget;
            meleeAttack.creature = components.creature;
            meleeAttack.liveMixin = components.liveMixin;
            meleeAttack.animator = components.creature.GetAnimator();

            var avoidObstacels = prefab.GetComponent<AvoidObstacles>();
            avoidObstacels.avoidanceIterations = 15;
            avoidObstacels.scanInterval = 0.5f;

            mouth.AddComponent<OnTouch>();
            lClawTrigger.AddComponent<OnTouch>();
            rClawTrigger.AddComponent<OnTouch>();
            AddClawAttack("LClaw", "swipeL", components);
            AddClawAttack("RClaw", "swipeR", components);

            components.locomotion.driftFactor = 0.4f;
            components.locomotion.maxAcceleration = 10f;
            }

        void AddClawAttack(string triggerName, string animationName, CreatureComponents components)
        {
            GulperMeleeAttack_Claw meleeAttack = prefab.AddComponent<GulperMeleeAttack_Claw>();
            meleeAttack.mouth = prefab.SearchChild(triggerName);
            meleeAttack.canBeFed = false;
            meleeAttack.biteInterval = 1f;
            meleeAttack.biteDamage = 50f;
            meleeAttack.eatHungerDecrement = 0.05f;
            meleeAttack.eatHappyIncrement = 0.1f;
            meleeAttack.biteAggressionDecrement = 0.02f;
            meleeAttack.biteAggressionThreshold = 0.1f;
            meleeAttack.lastTarget = components.lastTarget;
            meleeAttack.creature = components.creature;
            meleeAttack.liveMixin = components.liveMixin;
            meleeAttack.animator = components.creature.GetAnimator();
            meleeAttack.colliderName = triggerName;
            meleeAttack.animationTriggerName = animationName;
            meleeAttack.isBaby = true;
        }

    }
}
*/