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
                        new[] {"mouth_damage_trigger"},
                        new AttachableParasite(TechType.Warper, 6f),
                        new AttachableParasite(TechType.GhostLeviathan, 0.5f)
                        //new AttachableParasite(TechType.CrabSquid, 2f)
                    ) /*,
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
        },
        // ------------------------------------------------------------------
        {
            TechType.BoneShark, new AmalgamationSettings(new[]
                {
                    new ParasiteAttachPoint(
                        new string[] {"bone_shark_anim/root/neck/head"},
                        1f,
                        new Vector3(0, 270, 0),
                        true,
                        new[] {"Mouth"},
                        new AttachableParasite(TechType.Stalker, 0.8f),
                        new AttachableParasite(TechType.Eyeye, 3f),
                        new AttachableParasite(TechType.Warper, 0.5f),
                        new AttachableParasite(TechType.CrabSquid, 0.2f),
                        new AttachableParasite(TechType.Crash, 2f),
                        new AttachableParasite(TechType.Peeper, 2.5f),
                        new AttachableParasite(TechType.Jellyray, 0.5f),
                        new AttachableParasite(TechType.GhostRayBlue, 0.5f),
                        new AttachableParasite(TechType.Mesmer, 2f),
                        new AttachableParasite(TechType.CaveCrawler, 2f),
                        new AttachableParasite(TechType.Jumper, 2f),
                        new AttachableParasite(TechType.Bleeder, 8f),
                        new AttachableParasite(TechType.Sandshark, 1f)
                    )
                }
            )
        },
        // ------------------------------------------------------------------
        {
            TechType.Stalker, new AmalgamationSettings(new[]
                {
                    new ParasiteAttachPoint(
                        new string[] {"Stalker_02/root/neck/head"},
                        1f,
                        new Vector3(0, 90, 0),
                        true,
                        new[] {"Mouth"},
                        new AttachableParasite(TechType.Stalker, 0.8f),
                        new AttachableParasite(TechType.Eyeye, 3f),
                        new AttachableParasite(TechType.Warper, 0.5f),
                        new AttachableParasite(TechType.CrabSquid, 0.2f),
                        new AttachableParasite(TechType.Crash, 2f),
                        new AttachableParasite(TechType.Peeper, 2.5f),
                        new AttachableParasite(TechType.Jellyray, 0.5f),
                        new AttachableParasite(TechType.GhostRayBlue, 0.5f),
                        new AttachableParasite(TechType.Mesmer, 2f),
                        new AttachableParasite(TechType.CaveCrawler, 2f),
                        new AttachableParasite(TechType.Jumper, 2f),
                        new AttachableParasite(TechType.Bleeder, 8f),
                        new AttachableParasite(TechType.Sandshark, 1f)
                    )
                }
            )
        },
        // ------------------------------------------------------------------
        {
            TechType.Sandshark, new AmalgamationSettings(new[]
                {
                    new ParasiteAttachPoint(
                        new string[]
                        {
                            "models/sand_shark_01/sand_shark_rig_SSGlobalTSR/sand_shark_rig_MoveCTRL/sand_shark_rig_MainGRP/sand_shark_rig_SS_TailBack/sand_shark_rig_bn_dorselSpn_a01/sand_shark_rig_bn_dorselSpn_b01/sand_shark_rig_bn_dorselSpn_c01/sand_shark_rig_bn_dorselSpn_d01/sand_shark_rig_bn_head_a01/sand_shark_rig_bn_Jaw_a01"
                        },
                        1f,
                        new Vector3(0, 90, 0),
                        true,
                        new string[0],
                        new AttachableParasite(TechType.Stalker, 0.2f),
                        new AttachableParasite(TechType.Eyeye, 1f),
                        new AttachableParasite(TechType.Warper, 0.2f),
                        new AttachableParasite(TechType.CrabSquid, 0.04f),
                        new AttachableParasite(TechType.Crash, 0.3f),
                        new AttachableParasite(TechType.Peeper, 0.8f),
                        new AttachableParasite(TechType.Jellyray, 0.3f),
                        new AttachableParasite(TechType.GhostRayBlue, 0.1f),
                        new AttachableParasite(TechType.Mesmer, 0.8f),
                        new AttachableParasite(TechType.CaveCrawler, 0.9f),
                        new AttachableParasite(TechType.Jumper, 0.5f),
                        new AttachableParasite(TechType.Bleeder, 2f)
                    )
                }
            )
        },
        // ------------------------------------------------------------------
        {
            TechType.CrabSquid, new AmalgamationSettings(new[]
                {
                    new ParasiteAttachPoint(
                        new string[]
                        {
                            "models/Crab_Squid/crab_squid_skele/Root_jnt/inner_head_jnt/R_eyeStem_jnt1",
                            "models/Crab_Squid/crab_squid_skele/Root_jnt/inner_head_jnt/R_eyeStem_jnt2",
                            "models/Crab_Squid/crab_squid_skele/Root_jnt/inner_head_jnt/L_eyeStem_jnt1",
                            "models/Crab_Squid/crab_squid_skele/Root_jnt/inner_head_jnt/L_eyeStem_jnt2"
                        },
                        0.5f,
                        new Vector3(0, 90, 0),
                        true,
                        new string[0],
                        new AttachableParasite(TechType.Stalker, 0.8f),
                        new AttachableParasite(TechType.Eyeye, 3f),
                        new AttachableParasite(TechType.Warper, 0.5f),
                        new AttachableParasite(TechType.CrabSquid, 0.2f),
                        new AttachableParasite(TechType.Crash, 2f),
                        new AttachableParasite(TechType.Peeper, 2.5f),
                        new AttachableParasite(TechType.Jellyray, 0.5f),
                        new AttachableParasite(TechType.GhostRayBlue, 0.5f),
                        new AttachableParasite(TechType.Mesmer, 2f),
                        new AttachableParasite(TechType.CaveCrawler, 2f),
                        new AttachableParasite(TechType.Jumper, 2f),
                        new AttachableParasite(TechType.Bleeder, 8f),
                        new AttachableParasite(TechType.Sandshark, 1f)
                    ),
                    new ParasiteAttachPoint(
                        new string[]
                        {
                            "models/Crab_Squid/crab_squid_skele/Root_jnt/hip_jnt/L_leg1_jnt/L_leg1_jnt1/L_leg1_jnt2/L_leg1_jnt3/L_leg1_jnt4/L_leg1_jnt5",
                            "models/Crab_Squid/crab_squid_skele/Root_jnt/hip_jnt/L_leg2_jnt/L_leg2_jnt1/L_leg2_jnt2/L_leg2_jnt3/L_leg2_jnt4/L_leg2_jnt5",
                            "models/Crab_Squid/crab_squid_skele/Root_jnt/hip_jnt/L_leg3_jnt/L_leg3_jnt1/L_leg3_jnt2/L_leg3_jnt3/L_leg3_jnt4/L_leg3_jnt5",
                            "models/Crab_Squid/crab_squid_skele/Root_jnt/hip_jnt/L_leg4_jnt/L_leg4_jnt1/L_leg4_jnt2/L_leg4_jnt3/L_leg4_jnt4/L_leg4_jnt5",
                            "models/Crab_Squid/crab_squid_skele/Root_jnt/hip_jnt/R_leg1_jnt/R_leg1_jnt1/R_leg1_jnt2/R_leg1_jnt3/R_leg1_jnt4/R_leg1_jnt5",
                            "models/Crab_Squid/crab_squid_skele/Root_jnt/hip_jnt/R_leg2_jnt/R_leg2_jnt1/R_leg2_jnt2/R_leg2_jnt3/R_leg2_jnt4/R_leg2_jnt5",
                            "models/Crab_Squid/crab_squid_skele/Root_jnt/hip_jnt/R_leg3_jnt/R_leg3_jnt1/R_leg3_jnt2/R_leg3_jnt3/R_leg3_jnt4/R_leg3_jnt5",
                            "models/Crab_Squid/crab_squid_skele/Root_jnt/hip_jnt/R_leg4_jnt/R_leg4_jnt1/R_leg4_jnt2/R_leg4_jnt3/R_leg4_jnt4/R_leg4_jnt5",
                        },
                        0.3f,
                        new Vector3(0, 90, 0),
                        true,
                        new string[0],
                        new AttachableParasite(TechType.Stalker, 0.8f),
                        new AttachableParasite(TechType.Eyeye, 3f),
                        new AttachableParasite(TechType.Warper, 0.5f),
                        new AttachableParasite(TechType.CrabSquid, 0.2f),
                        new AttachableParasite(TechType.Crash, 2f),
                        new AttachableParasite(TechType.Peeper, 2.5f),
                        new AttachableParasite(TechType.Jellyray, 0.5f),
                        new AttachableParasite(TechType.GhostRayBlue, 0.5f),
                        new AttachableParasite(TechType.Mesmer, 2f),
                        new AttachableParasite(TechType.CaveCrawler, 2f),
                        new AttachableParasite(TechType.Jumper, 2f),
                        new AttachableParasite(TechType.Bleeder, 8f),
                        new AttachableParasite(TechType.Sandshark, 1f)
                    )
                }
            )
        },
        // ------------------------------------------------------------------
        {
            TechType.Shocker, new AmalgamationSettings(new[]
                {
                    new ParasiteAttachPoint(
                        new string[]
                        {
                            "shocker_anim/root/head_back"
                        },
                        1f,
                        new Vector3(0, 90, 0),
                        false,
                        new string[0],
                        new AttachableParasite(TechType.Stalker, 0.8f),
                        new AttachableParasite(TechType.Eyeye, 3f),
                        new AttachableParasite(TechType.Warper, 0.5f),
                        new AttachableParasite(TechType.CrabSquid, 0.2f),
                        new AttachableParasite(TechType.Crash, 2f),
                        new AttachableParasite(TechType.Peeper, 2.5f),
                        new AttachableParasite(TechType.Jellyray, 0.5f),
                        new AttachableParasite(TechType.GhostRayBlue, 0.5f),
                        new AttachableParasite(TechType.Mesmer, 2f),
                        new AttachableParasite(TechType.CaveCrawler, 2f),
                        new AttachableParasite(TechType.Jumper, 2f),
                        new AttachableParasite(TechType.Bleeder, 8f),
                        new AttachableParasite(TechType.Sandshark, 1f)
                    ),
                    new ParasiteAttachPoint(
                        new string[]
                        {
                            "shocker_anim/root/head_back/tail_1/segment1_front/segment1_back/tail_2/segment2_front/segment2_back/tail_3/segment3_front/segment3_back/tail_4/segment4_front/segment4_back/tail_5/segment5_front/segment5_back/tail_6/segment6_front/segment6_back/tail_7"
                            //"shocker_anim/root/head_back/tail_1/segment1_front/segment1_back/tail_2/segment2_front/segment2_back/tail_3/segment3_front/segment3_back/tail_4/segment4_front/segment4_back/tail_5/segment5_front/segment5_back/tail_6/segment6_front/segment6_back/tail_7/segment7_front/segment7_back/tail_8/segment8_front/segment8_back/tail_9"
                        },
                        1f,
                        new Vector3(0, 90, 0),
                        true,
                        new string[0],
                        new AttachableParasite(TechType.Stalker, 0.8f),
                        new AttachableParasite(TechType.Eyeye, 3f),
                        new AttachableParasite(TechType.Warper, 0.5f),
                        new AttachableParasite(TechType.CrabSquid, 0.2f),
                        new AttachableParasite(TechType.Crash, 2f),
                        new AttachableParasite(TechType.Peeper, 2.5f),
                        new AttachableParasite(TechType.Jellyray, 0.5f),
                        new AttachableParasite(TechType.GhostRayBlue, 0.5f),
                        new AttachableParasite(TechType.Mesmer, 2f),
                        new AttachableParasite(TechType.CaveCrawler, 2f),
                        new AttachableParasite(TechType.Jumper, 2f),
                        new AttachableParasite(TechType.Bleeder, 8f),
                        new AttachableParasite(TechType.Sandshark, 1f)
                    )
                }
            )
        },
        // ------------------------------------------------------------------
        {
            TechType.GhostRayBlue, new AmalgamationSettings(new[]
                {
                    new ParasiteAttachPoint(
                        new string[]
                        {
                            "model/ghost_ray/root_jnt/Tail_phy/tail_jnt/Tail_phy1/tail1_jnt/Tail_phy2/tail2_jnt/Tail_phy3/tail3_jnt/Tail_phy4/tail4_jnt/Tail_phy5/tail5_jnt/Tail_phy6"
                        },
                        1f,
                        new Vector3(0, 90, 0),
                        true,
                        new string[0],
                        new AttachableParasite(TechType.Stalker, 0.8f),
                        new AttachableParasite(TechType.Eyeye, 3f),
                        new AttachableParasite(TechType.Warper, 0.5f),
                        new AttachableParasite(TechType.CrabSquid, 0.2f),
                        new AttachableParasite(TechType.Crash, 2f),
                        new AttachableParasite(TechType.Peeper, 2.5f),
                        new AttachableParasite(TechType.Jellyray, 0.5f),
                        new AttachableParasite(TechType.GhostRayBlue, 0.5f),
                        new AttachableParasite(TechType.Mesmer, 2f),
                        new AttachableParasite(TechType.CaveCrawler, 2f),
                        new AttachableParasite(TechType.Jumper, 2f),
                        new AttachableParasite(TechType.Bleeder, 8f),
                        new AttachableParasite(TechType.Sandshark, 1f)
                    )
                }
            )
        },
        // ------------------------------------------------------------------
        {
            TechType.GhostRayRed, new AmalgamationSettings(new[]
                {
                    new ParasiteAttachPoint(
                        new string[]
                        {
                            "model/ghost_ray_red/root_jnt"
                        },
                        1f,
                        new Vector3(0, 90, 0),
                        false,
                        new string[0],
                        new AttachableParasite(TechType.Stalker, 0.8f),
                        new AttachableParasite(TechType.Eyeye, 3f),
                        new AttachableParasite(TechType.Warper, 0.5f),
                        new AttachableParasite(TechType.CrabSquid, 0.2f),
                        new AttachableParasite(TechType.Crash, 2f),
                        new AttachableParasite(TechType.Peeper, 2.5f),
                        new AttachableParasite(TechType.Jellyray, 0.5f),
                        new AttachableParasite(TechType.GhostRayBlue, 0.5f),
                        new AttachableParasite(TechType.Mesmer, 2f),
                        new AttachableParasite(TechType.CaveCrawler, 2f),
                        new AttachableParasite(TechType.Jumper, 2f),
                        new AttachableParasite(TechType.Bleeder, 8f),
                        new AttachableParasite(TechType.Sandshark, 1f)
                    ),
                    new ParasiteAttachPoint(
                        new string[]
                        {
                            "model/ghost_ray_red/root_jnt/Tail_phy/tail_jnt/Tail_phy1/tail1_jnt/Tail_phy2/tail2_jnt/Tail_phy3/tail3_jnt/Tail_phy4/tail4_jnt/Tail_phy5/tail5_jnt/Tail_phy6"
                        },
                        1f,
                        new Vector3(0, 90, 0),
                        true,
                        new string[0],
                        new AttachableParasite(TechType.Stalker, 0.8f),
                        new AttachableParasite(TechType.Eyeye, 3f),
                        new AttachableParasite(TechType.Warper, 0.5f),
                        new AttachableParasite(TechType.CrabSquid, 0.2f),
                        new AttachableParasite(TechType.Crash, 2f),
                        new AttachableParasite(TechType.Peeper, 2.5f),
                        new AttachableParasite(TechType.Jellyray, 0.5f),
                        new AttachableParasite(TechType.GhostRayBlue, 0.5f),
                        new AttachableParasite(TechType.Mesmer, 2f),
                        new AttachableParasite(TechType.CaveCrawler, 2f),
                        new AttachableParasite(TechType.Jumper, 2f),
                        new AttachableParasite(TechType.Bleeder, 8f),
                        new AttachableParasite(TechType.Sandshark, 1f)
                    )
                }
            )
        },
        // ------------------------------------------------------------------
        {
            TechType.LavaLizard, new AmalgamationSettings(new[]
                {
                    new ParasiteAttachPoint(
                        new string[]
                        {
                            "lava_lizard/root/clav_left/shoulder_left",
                            "lava_lizard/root/clav_right/shoulder_right"
                        },
                        0.8f,
                        new Vector3(0, 90, 0),
                        true,
                        new string[0],
                        new AttachableParasite(TechType.Eyeye, 3f),
                        new AttachableParasite(TechType.Warper, 0.5f),
                        new AttachableParasite(TechType.CrabSquid, 0.2f),
                        new AttachableParasite(TechType.Crash, 2f),
                        new AttachableParasite(TechType.Peeper, 2.5f),
                        new AttachableParasite(TechType.Jellyray, 0.5f),
                        new AttachableParasite(TechType.GhostRayBlue, 0.5f),
                        new AttachableParasite(TechType.Mesmer, 2f),
                        new AttachableParasite(TechType.CaveCrawler, 2f),
                        new AttachableParasite(TechType.Jumper, 2f),
                        new AttachableParasite(TechType.Bleeder, 8f),
                        new AttachableParasite(TechType.Sandshark, 1f)
                    )
                }
            )
        },
        // ------------------------------------------------------------------
        {
            TechType.Warper, new AmalgamationSettings(new[]
                {
                    new ParasiteAttachPoint(
                        new string[]
                        {
                            "warper_anim/root/neck"
                        },
                        1f,
                        new Vector3(0, 90, 0),
                        true,
                        new string[0],
                        new AttachableParasite(TechType.Stalker, 0.2f),
                        new AttachableParasite(TechType.Eyeye, 1f),
                        new AttachableParasite(TechType.Warper, 0.2f),
                        new AttachableParasite(TechType.CrabSquid, 0.04f),
                        new AttachableParasite(TechType.Crash, 0.3f),
                        new AttachableParasite(TechType.Peeper, 0.8f),
                        new AttachableParasite(TechType.Jellyray, 0.3f),
                        new AttachableParasite(TechType.GhostRayBlue, 0.1f),
                        new AttachableParasite(TechType.Mesmer, 0.8f),
                        new AttachableParasite(TechType.CaveCrawler, 0.9f),
                        new AttachableParasite(TechType.Jumper, 0.5f),
                        new AttachableParasite(TechType.Bleeder, 2f)
                    )
                }
            )
        },
        // ------------------------------------------------------------------
        {
            TechType.CaveCrawler, new AmalgamationSettings(new[]
                {
                    new ParasiteAttachPoint(
                        new string[]
                        {
                            "cave_crawler_01/cave_crawler_01"
                        },
                        1f,
                        new Vector3(0, 90, 0),
                        false,
                        new string[0],
                        new AttachableParasite(TechType.Stalker, 0.2f),
                        new AttachableParasite(TechType.Eyeye, 1f),
                        new AttachableParasite(TechType.Warper, 0.2f),
                        new AttachableParasite(TechType.CrabSquid, 0.04f),
                        new AttachableParasite(TechType.Crash, 0.3f),
                        new AttachableParasite(TechType.Peeper, 0.8f),
                        new AttachableParasite(TechType.Jellyray, 0.3f),
                        new AttachableParasite(TechType.GhostRayBlue, 0.1f),
                        new AttachableParasite(TechType.Mesmer, 0.8f),
                        new AttachableParasite(TechType.CaveCrawler, 0.9f),
                        new AttachableParasite(TechType.Jumper, 0.5f),
                        new AttachableParasite(TechType.Bleeder, 2f)
                    )
                }
            )
        },
        // ------------------------------------------------------------------
        {
            TechType.Shuttlebug, new AmalgamationSettings(new[]
                {
                    new ParasiteAttachPoint(
                        new string[]
                        {
                            "Cave_Crawler_03/Cave_Crawler_blood_01"
                        },
                        1f,
                        new Vector3(0, 90, 0),
                        false,
                        new string[0],
                        new AttachableParasite(TechType.Stalker, 0.2f),
                        new AttachableParasite(TechType.Eyeye, 1f),
                        new AttachableParasite(TechType.Warper, 0.2f),
                        new AttachableParasite(TechType.CrabSquid, 0.04f),
                        new AttachableParasite(TechType.Crash, 0.3f),
                        new AttachableParasite(TechType.Peeper, 0.8f),
                        new AttachableParasite(TechType.Jellyray, 0.3f),
                        new AttachableParasite(TechType.GhostRayBlue, 0.1f),
                        new AttachableParasite(TechType.Mesmer, 0.8f),
                        new AttachableParasite(TechType.CaveCrawler, 0.9f),
                        new AttachableParasite(TechType.Jumper, 0.5f),
                        new AttachableParasite(TechType.Bleeder, 2f)
                    )
                }
            )
        }
    };
}