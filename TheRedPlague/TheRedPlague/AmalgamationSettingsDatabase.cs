using System.Collections.Generic;
using UnityEngine;

namespace TheRedPlague;

public static class AmalgamationSettingsDatabase
{
    public static readonly Dictionary<TechType, AmalgamationSettings> SettingsList = new()
    {
        // ------------------------------------------------------------------
        {
            TechType.ReaperLeviathan, new AmalgamationSettings(new[]
                {
                    new ParasiteAttachPoint(
                        new string[] {"reaper_leviathan/root/neck/head"},
                        0.9f,
                        new Vector3(0, 90, 0),
                        true,
                        new[] {"mouth_damage_trigger", "cine_player_loc8ion", "cine_vehicle_loc8ion"},
                        new AttachableParasite(TechType.Gasopod, 1.5f),
                        new AttachableParasite(TechType.Stalker, 3f),
                        new AttachableParasite(TechType.Eyeye, 15),
                        new AttachableParasite(TechType.Warper, 3f),
                        new AttachableParasite(TechType.SpineEel, 3f),
                        new AttachableParasite(TechType.CrabSquid, 1.5f),
                        new AttachableParasite(TechType.Crash, 8f),
                        new AttachableParasite(TechType.Peeper, 15)
                    ),
                    new ParasiteAttachPoint(
                        new string[]
                        {
                            "reaper_leviathan/root/clav_left", "reaper_leviathan/root/clav_right",
                            "reaper_leviathan/root/spine1_phys/spine1",
                            "reaper_leviathan/root/spine1_phys/spine1/spine1p5_phys/spline1p5/spline2_phys/Spine2",
                            "reaper_leviathan/root/spine1_phys/spine1/spine1p5_phys/spline1p5/spline2_phys/Spine2/spline3_phys/spine3/tail_midBase_phys/tail_midBase/tail_midMid_phys/tail_midMid/fin_leftAnal",
                            "reaper_leviathan/root/spine1_phys/spine1/spine1p5_phys/spline1p5/spline2_phys/Spine2/spline3_phys/spine3/tail_midBase_phys/tail_midBase/tail_midMid_phys/tail_midMid/fin_rightAnal",
                            "reaper_leviathan/root/spine1_phys/spine1/spine1p5_phys/spline1p5/spline2_phys/Spine2/spline3_phys/spine3/tail_midBase_phys/tail_midBase/tail_midMid_phys/tail_midMid/tail_midEnd_phys/tail_midEnd"
                        },  
                        0.4f,
                        new Vector3(0, 270, 0),
                        true,
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
                        new AttachableParasite(TechType.GhostRayBlue, 1f),
                        new AttachableParasite(TechType.Mesmer, 4f),
                        new AttachableParasite(TechType.CaveCrawler, 5f),
                        new AttachableParasite(TechType.Jumper, 4f),
                        new AttachableParasite(TechType.ReaperLeviathan, 0.3f)
                    )
                }
            )
        },
        // ------------------------------------------------------------------
        {
            TechType.GhostLeviathan, new AmalgamationSettings(new[]
                {
                    new ParasiteAttachPoint(
                        new string[]
                        {
                            "model/Ghost_Leviathan_anim/root/tail_1_phys/tail_1",
                            "model/Ghost_Leviathan_anim/root/tail_1_phys/tail_1/tail_2_phys/tail_2",
                            "model/Ghost_Leviathan_anim/root/tail_1_phys/tail_1/tail_2_phys/tail_2/tail_3_phys/tail_3",
                            "model/Ghost_Leviathan_anim/root/tail_1_phys/tail_1/tail_2_phys/tail_2/tail_3_phys/tail_3/tail_4_phys/tail_4",
                            "model/Ghost_Leviathan_anim/root/tail_1_phys/tail_1/tail_2_phys/tail_2/tail_3_phys/tail_3/tail_4_phys/tail_4/tail_5_phys/tail_5",
                            "model/Ghost_Leviathan_anim/root/tail_1_phys/tail_1/tail_2_phys/tail_2/tail_3_phys/tail_3/tail_4_phys/tail_4/tail_5_phys/tail_5/tail_6_phys/tail_6",
                            "model/Ghost_Leviathan_anim/root/tail_1_phys/tail_1/tail_2_phys/tail_2/tail_3_phys/tail_3/tail_4_phys/tail_4/tail_5_phys/tail_5/tail_6_phys/tail_6/tail_7_phys/tail_7",
                            "model/Ghost_Leviathan_anim/root/tail_1_phys/tail_1/tail_2_phys/tail_2/tail_3_phys/tail_3/tail_4_phys/tail_4/tail_5_phys/tail_5/tail_6_phys/tail_6/tail_7_phys/tail_7/tail_8_phys/tail_8",
                            "model/Ghost_Leviathan_anim/root/tail_1_phys/tail_1/tail_2_phys/tail_2/tail_3_phys/tail_3/tail_4_phys/tail_4/tail_5_phys/tail_5/tail_6_phys/tail_6/tail_7_phys/tail_7/tail_8_phys/tail_8/tail_9_phys/tail_9"
                        },  
                        0.3f,
                        new Vector3(0, 270, 0),
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
            )
        },
        // ------------------------------------------------------------------
        {
            TechType.SeaDragon, new AmalgamationSettings(new[]
                {
                    new ParasiteAttachPoint(
                        new string[] {"Sea_Dragon_wholeBody_anim/chest/neck/head"},
                        0.9f,
                        new Vector3(0, 270, 0),
                        true,
                        new[] { "mouth_damage_trigger" },
                        new AttachableParasite(TechType.Warper, 6f),
                        new AttachableParasite(TechType.GhostLeviathan, 0.5f),
                        new AttachableParasite(TechType.CrabSquid, 2f),
                        new AttachableParasite(TechType.Crash, 20f),
                        new AttachableParasite(TechType.GarryFish, 18f)
                    )/*,
                    new ParasiteAttachPoint(
                        new string[]
                        {
                            "reaper_leviathan/root/clav_left", "reaper_leviathan/root/clav_right",
                            "reaper_leviathan/root/spine1_phys/spine1",
                            "reaper_leviathan/root/spine1_phys/spine1/spine1p5_phys/spline1p5/spline2_phys/Spine2",
                            "reaper_leviathan/root/spine1_phys/spine1/spine1p5_phys/spline1p5/spline2_phys/Spine2/spline3_phys/spine3/tail_midBase_phys/tail_midBase/tail_midMid_phys/tail_midMid/fin_leftAnal",
                            "reaper_leviathan/root/spine1_phys/spine1/spine1p5_phys/spline1p5/spline2_phys/Spine2/spline3_phys/spine3/tail_midBase_phys/tail_midBase/tail_midMid_phys/tail_midMid/fin_rightAnal",
                            "reaper_leviathan/root/spine1_phys/spine1/spine1p5_phys/spline1p5/spline2_phys/Spine2/spline3_phys/spine3/tail_midBase_phys/tail_midBase/tail_midMid_phys/tail_midMid/tail_midEnd_phys/tail_midEnd"
                        },  
                        0.4f,
                        new Vector3(0, 270, 0),
                        true,
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
                        new AttachableParasite(TechType.GhostRayBlue, 1f),
                        new AttachableParasite(TechType.Mesmer, 4f),
                        new AttachableParasite(TechType.CaveCrawler, 5f),
                        new AttachableParasite(TechType.Jumper, 4f),
                        new AttachableParasite(TechType.ReaperLeviathan, 0.3f)
                    )*/
                }
            )
        }
    };
}