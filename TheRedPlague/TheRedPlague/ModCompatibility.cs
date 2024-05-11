using Nautilus.Handlers;
using UnityEngine;

namespace TheRedPlague;

public static class ModCompatibility
{
    public static void PatchCompatibility()
    {
        if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.aci.thesilence"))
        {
            PatchSilenceCompatibility();
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
}