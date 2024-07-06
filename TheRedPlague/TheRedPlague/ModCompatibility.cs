using System.Collections;
using Nautilus.Handlers;
using TheRedPlague.PrefabFiles;
using UnityEngine;

namespace TheRedPlague;

public static class ModCompatibility
{
    public static bool SilenceModInstalled { get; private set; }
    public static bool BloopAndBlazaModInstalled { get; private set; }
    public static bool TurretsModInstalled { get; private set; }
    public static TechType TurretTechType { get; private set; }
    
    public static void PatchCompatibility()
    {
        if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.aci.thesilence"))
        {
            SilenceModInstalled = true;
            PatchSilenceCompatibility();
        }
        if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.lee23.bloopandblaza"))
        {
            BloopAndBlazaModInstalled = true;
            PatchBloopCompatibility();
        }
        if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("UnknownModdingLib"))
        {
            TurretsModInstalled = true;
            PatchTurretsCompatibility();
        }
    }

    private static void PatchTurretsCompatibility()
    {
        if (EnumHandler.TryGetValue<TechType>("alterra_turret", out var turretTechType))
        {
            TurretTechType = turretTechType;
            KnownTechHandler.RemoveDefaultUnlock(TurretTechType);
            TurretConsolePrefab.Register();
        }
    }

    private static void PatchSilenceCompatibility()
    {
        if (!EnumHandler.TryGetValue<TechType>("Silence", out var silenceTechType))
            return;

        AmalgamationSettingsDatabase.SettingsList.Add(silenceTechType, new AmalgamationSettings(new[]
            {
                new ParasiteAttachPoint(
                    new string[] {"SilenceModel/Armature/Root/Chest/UpperChest/Neck/Neck1/Head"},
                    1f,
                    new Vector3(270, 0, 0),
                    true,
                    new string[0],
                    new AttachableParasite(TechType.Gasopod, 4f),
                    new AttachableParasite(TechType.Stalker, 6f),
                    new AttachableParasite(TechType.Warper, 6f),
                    new AttachableParasite(TechType.SpineEel, 6f),
                    new AttachableParasite(TechType.CrabSquid, 3f)
                ),
                new ParasiteAttachPoint(
                        new string[]
                        {
                            "SilenceModel/Armature/Root/Chest/LeftArm",
                            "SilenceModel/Armature/Root/Chest/RightArm",
                            "SilenceModel/Armature/Root/Chest/Tail/Tail.001/Tail.002",
                            "SilenceModel/Armature/Root/Chest/Tail/Tail.001/Tail.002/LargeTailWing_L",
                            "SilenceModel/Armature/Root/Chest/Tail/Tail.001/Tail.002/LargeTailWing_R",
                            "SilenceModel/Armature/Root/Chest/Tail/Tail.001/Tail.002/Tail.003/Tail.004",
                            "SilenceModel/Armature/Root/Chest/Tail/Tail.001/Tail.002/Tail.003/Tail.004/Tail.005/Tail.006/Tail.007",
                            "SilenceModel/Armature/Root/Chest/Tail/Tail.001/Tail.002/Tail.003/Tail.004/Tail.005/Tail.006/Tail.007/MediumTailWing_L",
                            "SilenceModel/Armature/Root/Chest/Tail/Tail.001/Tail.002/Tail.003/Tail.004/Tail.005/Tail.006/Tail.007/MediumTailWing_R",
                            "SilenceModel/Armature/Root/Chest/Tail/Tail.001/Tail.002/Tail.003/Tail.004/Tail.005/Tail.006/Tail.007/Tail.008/Tail.009",
                            "SilenceModel/Armature/Root/Chest/Tail/Tail.001/Tail.002/Tail.003/Tail.004/Tail.005/Tail.006/Tail.007/Tail.008/Tail.009/Tail.010/Tail.011",
                            "SilenceModel/Armature/Root/Chest/Tail/Tail.001/Tail.002/Tail.003/Tail.004/Tail.005/Tail.006/Tail.007/Tail.008/Tail.009/Tail.010/Tail.011/Tail.012/Tail.013/Tail.014/Tail.015",
                        },  
                        0.2f,
                        new Vector3(270, 0, 0),
                        false,
                        new string[0],
                        new AttachableParasite(TechType.Gasopod, 1.5f),
                        new AttachableParasite(TechType.Stalker, 2f),
                        new AttachableParasite(TechType.Eyeye, 10f),
                        new AttachableParasite(TechType.Warper, 2f),
                        new AttachableParasite(TechType.SpineEel, 1f),
                        new AttachableParasite(TechType.CrabSquid, 0.8f),
                        new AttachableParasite(TechType.Crash, 8f),
                        new AttachableParasite(TechType.Peeper, 7f),
                        new AttachableParasite(TechType.Jellyray, 1f),
                        new AttachableParasite(TechType.SeaTreader, 1f),
                        new AttachableParasite(TechType.GhostRayBlue, 1f),
                        new AttachableParasite(TechType.Mesmer, 4f),
                        new AttachableParasite(TechType.CaveCrawler, 5f),
                        new AttachableParasite(TechType.Jumper, 4f),
                        new AttachableParasite(TechType.ReaperLeviathan, 0.3f),
                        new AttachableParasite(TechType.BoneShark, 1.5f)
                    )
            }
        ));
    }

    private static void PatchBloopCompatibility()
    {
        if (EnumHandler.TryGetValue<TechType>("BlazaLeviathan", out var blazaLeviathanTechType))
        {
            InfectionSettingsDatabase.InfectionSettingsList.Add(blazaLeviathanTechType, new InfectionSettings(new Color(5, 6, 5), 0.1f, Vector3.one, Vector3.zero));
        }

        if (EnumHandler.TryGetValue<TechType>("Bloop", out var bloopTechType))
        {
            InfectionSettingsDatabase.InfectionSettingsList.Add(bloopTechType, new InfectionSettings(new Color(2, 3, 5), -1.9f, new Vector3(10, 10, 10), new Vector3(0.142f, 0.476f, 0.333f)));
            AmalgamationSettingsDatabase.CustomModificationsList.Add(bloopTechType, AmalgamateBloop);
        }
    }

    private static IEnumerator AmalgamateBloop(GameObject obj)
    {
        foreach (var light in obj.GetComponentsInChildren<Light>()) light.color = Color.red;
        var renderer = obj.GetComponentInChildren<Renderer>();
        var materials = renderer.materials;
        foreach (var material in materials)
        {
            material.SetColor("_GlowColor", Color.red);
        }

        renderer.materials = materials;
        var reaperRequest = CraftData.GetPrefabForTechTypeAsync(TechType.ReaperLeviathan);
        yield return reaperRequest;
        var reaperPrefab = reaperRequest.GetResult();
        if (obj == null) yield break;
        (Vector3, Vector3, Vector3)[] locations = new[]
        {
            (new Vector3(-1.6f, 7.3f, 10.2f), new Vector3(-13, 0, 0), Vector3.one * 1.1f),
            (new Vector3(-1.58f, 11.03f, 5.79f), new Vector3(-61, 0, 0), Vector3.one * 0.6f),
            (new Vector3(4.98f, 5.392f, 10.348f), new Vector3(-10.2f, 7, 0), Vector3.one * 0.9f),
            (new Vector3(-4.98f, 5.392f, 10.348f), new Vector3(-10.2f, -7, 0), Vector3.one * 1.2f),
            (new Vector3(-17f, -0.9f, 2.7f), new Vector3(0, -90, 0), Vector3.one * 1.3f),
            (new Vector3(-17f, -0.9f, 2.7f), new Vector3(0, -90, 0), Vector3.one * 1.4f),
            (new Vector3(-4.5f, -4.8f, 8.81f), new Vector3(20, -5, 0), Vector3.one * 0.9f),
            (new Vector3(4.5f, -4.8f, 8.81f), new Vector3(20, 5, 0), Vector3.one * 0.8f),
            (new Vector3(-1.58f, -10.69f, -7.97f), new Vector3(97, 0, 0), Vector3.one * 1),
            (new Vector3(0, 1, -10), new Vector3(0, 0, 0), Vector3.one * 2f),
        };
        
        foreach (var location in locations)
        {
            if (Random.value < 0.5f) continue;
            var head = UWE.Utils.InstantiateDeactivated(reaperPrefab);
            head.transform.Find("reaper_leviathan/root/spine1_phys").localScale = Vector3.one * 0.001f;
            Object.Destroy(head.GetComponent<LargeWorldEntity>());
            Object.Destroy(head.GetComponent<PrefabIdentifier>());
            head.SetActive(true);
            head.transform.parent = obj.transform;
            head.GetComponent<Rigidbody>().isKinematic = true;
            head.transform.localPosition = location.Item1;
            head.transform.localEulerAngles = location.Item2;
            head.transform.localScale = location.Item3;
            head.GetComponent<Creature>().SetScale(location.Item3.x);
            ZombieManager.Zombify(head);
            foreach (var collider in head.GetComponentsInChildren<Collider>())
            {
                if (!collider.isTrigger) collider.enabled = false;
            }
            yield return null;
            LargeWorld.main.streamer.cellManager.UnregisterEntity(head);
        }
    }
}