using System.Collections.Generic;
using UnityEngine;

namespace TheRedPlague;

public static class InfectionSettingsDatabase
{
    public static readonly Dictionary<TechType, InfectionSettings> InfectionSettingsList = new()
    {
        {TechType.ReaperLeviathan, new InfectionSettings(new Color(2, 2, 2), -0.9f, new Vector3(4, 2, 7))},
        {TechType.Gasopod, new InfectionSettings(new Color(3, 3, 3), -0.7f, new Vector3(1, 1, 1), new Vector3(0.476190f, 0.596191f, 0.620476f))},
        {TechType.Skyray, new InfectionSettings(new Color(0.860294f, 0.860294f, 0.860294f), -0.1f, new Vector3(1.5f, 1.5f, 1.5f), new Vector3(0, 0, 0))},
        {TechType.Peeper, new InfectionSettings(new Color(3, 3, 3), -0.1f, new Vector3(1.5f, 1.5f, 1.5f), new Vector3(0, 0, 0))},
        {TechType.RabbitRay, new InfectionSettings(new Color(1, 1, 1), -0.3f, new Vector3(2, 2, 2), new Vector3(0, 0.28f, 0.32f))},
        {TechType.Stalker, new InfectionSettings(new Color(2, 2.5f, 2), -0.2f, new Vector3(1, 1, 5), new Vector3(0.47f, 0.52f, 0.29f))},
        {TechType.Shocker, new InfectionSettings(new Color(4, 4, 4), -0.3f, new Vector3(1, 2, 4), new Vector3(0.14f, 0.42f, -0.4f))},
        {TechType.GhostLeviathan, new InfectionSettings(new Color(1, 1, 1), 3f, new Vector3(5, 5, 20), new Vector3(0, 0, 0))},
        {TechType.GhostLeviathanJuvenile, new InfectionSettings(new Color(1, 1, 1), 3f, new Vector3(5, 5, 20), new Vector3(0, 0, 0))},
        {TechType.BoneShark, new InfectionSettings(new Color(0.642857f, 1, 0.642857f), 0.5f, new Vector3(1, 1, 3), new Vector3(0.737143f, 0.224762f, 0.531428f))},
        {TechType.Crabsnake, new InfectionSettings(new Color(0.428572f, 0.619048f, 0.571429f), -0.3f, new Vector3(0.3f, 0.3f, 3f), new Vector3(0.07f, 0.4f, 0.09f))},
        {TechType.CrabSquid, new InfectionSettings(new Color(5, 5, 5), 1f, new Vector3(3, 4, 3), new Vector3(-0.09f, 0.37f, 0.58f))},
        {TechType.LavaLizard, new InfectionSettings(new Color(4, 4, 4), -0.2f, new Vector3(0.75f, 2, 0.6f), new Vector3(0.48f, 1, 0.14f))},
        {TechType.SpineEel, new InfectionSettings(new Color(5, 5, 5), 0.1f, new Vector3(0.75f, 2.5f, 0.75f), new Vector3(0.76f, 0.714f, 0.33f))},
        {TechType.Sandshark, new InfectionSettings(new Color(2, 2.2f, 1.5f), 1f, new Vector3(1.13f, 1.27f, 0.76f), new Vector3(0.01f, 0.8f, 0.8f))},
        {TechType.GhostRayRed, new InfectionSettings(new Color(0.9f, 2, 2), 1f, new Vector3(1.6f, 0.2f, 1.5f), new Vector3(0.2f, 0f, 0f))},
        {TechType.GhostRayBlue, new InfectionSettings(new Color(1f, 1f, 1f), 1f, new Vector3(1.6f, 0.2f, 1.5f), new Vector3(0.2f, 0f, 0f))},
        {TechType.Eyeye, new InfectionSettings(new Color(3, 3, 3), -0.1f, new Vector3(0.45f, 0.45f, 0.45f), new Vector3(0.21f, 0.08f, 0.26f))},
        {TechType.HoleFish, new InfectionSettings(new Color(5, 5, 5), 0.1f, new Vector3(0.2f, 0.61f, 0.4f), new Vector3(0.75f, 0.78f, 1f))},
        {TechType.Hoopfish, new InfectionSettings(new Color(4, 4, 4), 0.05f, new Vector3(0.95f, 2, 2), new Vector3(0.67f, 0.48f, 0.05f))},
        {TechType.Hoverfish, new InfectionSettings(new Color(0, 0, 0), 0.05f, new Vector3(3, 3, 3), new Vector3(0, 0, 0))},
        {TechType.Oculus, new InfectionSettings(new Color(5, 5, 5), -0.1f, new Vector3(0.2f, 0.6f, 0.8f), new Vector3(0.93f, 0.21f, 0.47f))},
        {TechType.Spadefish, new InfectionSettings(new Color(4, 2, 2), -0.1f, new Vector3(1 ,1 ,1), new Vector3(0, 0, 0))},
        {TechType.LavaLarva, new InfectionSettings(new Color(5, 5, 5), 0.3f, new Vector3(0.3f, 0.2f, 0.6f), new Vector3(0.53f, 0.14f, 0.71f))},
        {TechType.Rockgrub, new InfectionSettings(new Color(1, 1, 1), 0.03f, new Vector3(1, 1, 1), new Vector3(0, 0, 0))},
        {TechType.Jumper, new InfectionSettings(new Color(1, 1, 1), -0.03f, new Vector3(0.3f, 0.5f, 0.3f), new Vector3(-0.33f, -0.58f, 0.47f))},
        {TechType.SeaDragon, new InfectionSettings(new Color(1, 1, 1), 1f, new Vector3(1, 1, 3), new Vector3(0.69f, 0.38f, 0.14f))},
        {TechType.Warper, new InfectionSettings(new Color(2, 4, 2), -0.5f, new Vector3(0.8f, 1.5f, 1.75f), new Vector3(0.21f, 0.38f, 0.24f))},
        // REEF BACK
        // SEA TREADER
        // SEA EMPEROR & JUVENILES
    };
}