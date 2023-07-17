using UnityEngine;

namespace AggressiveFauna;

public static class AggressionSettings
{
    public static bool ApplyConfigDuringDayTime { get { return Config.Instance.AffectDaytime; } }

    public static bool ApplyConfigDuringNightTime { get { return Config.Instance.AffectNighttime; } }

    public static int SearchRingScale { get { return CalculateSearchRingScale(); } }

    public static float MaxDistanceScale { get { return ScaleFloatWithTimeOfDay(1f, Config.Instance.DetectionRadiusMultiplier); } }

    public static bool CanSeeThroughTerrain { get { return ScaleBoolWithTimeOfDay(false, Config.Instance.CanSeeThroughTerrain); } }

    public static bool DisableFleeing { get { return ScaleBoolWithTimeOfDay(false, Config.Instance.DisableFleeing); } }

    public static bool DisableElectricityFlee { get { return ScaleBoolWithTimeOfDay(false, Config.Instance.DisableFleeingFromElectricity); } }

    public static bool CanFeed { get { return ScaleBoolWithTimeOfDay(false, !Config.Instance.DisableFeeding); } }

    public static float PlayerPrioritizationMultiplier { get { return ScaleFloatWithTimeOfDay(1f, Config.Instance.PlayerPrioritization); } }

    public static float AggressionMultiplier { get { return ScaleFloatWithTimeOfDay(1f, Config.Instance.AggressionMultiplier); } }

    public static float FOVMultiplier { get { return ScaleFloatWithTimeOfDay(1f, Config.Instance.FOVScale); } }

    public static float AttackDurationScale { get { return ScaleFloatWithTimeOfDay(1f, Config.Instance.AttackDurationScale); } }

    public static float RememberTargetTimeScale { get { return ScaleFloatWithTimeOfDay(1f, Config.Instance.RememberTargetTimeScale); } }

    public static float AttackCooldownScale { get { return ScaleFloatWithTimeOfDay(1f, Config.Instance.AttackCooldownPercentageNormalized); } }

    public static float AttackChargeVelocityScale { get { return ScaleFloatWithTimeOfDay(1f, Config.Instance.AttackChargeVelocityScale); } }

    public static float BiteCooldownScale { get { return ScaleFloatWithTimeOfDay(1f, Config.Instance.BiteCooldownPercentageNormalized); } }

    public static bool AllowFriends { get { return ScaleBoolWithTimeOfDay(true, !Config.Instance.DisableFeeding); } }

    public static bool CanSeeInsideBases { get { return ScaleBoolWithTimeOfDay(false, Config.Instance.CanSeeThroughBases); } }

    public static bool AttackEmptyVehicles { get { return ScaleBoolWithTimeOfDay(false, Config.Instance.AttackUnoccupiedVehicles); } }

    public static bool AlwaysBiteVehicles { get { return ScaleBoolWithTimeOfDay(false, Config.Instance.AlwaysBiteVehicles); } }

    public static bool AlwaysBiteCyclops { get { return ScaleBoolWithTimeOfDay(false, Config.Instance.AlwaysBiteCyclops); } }

    // constants

    private const float kMinDayLightScalar = 0.1f;
    private const float kMaxDayLightScalar = 0.85f;
    private const int kSearchRingScaleLimit = 3;

    // logic

    private static bool ScaleBoolWithTimeOfDay(bool unmoddedValue, bool configValue)
    {
        bool isDay = GetIsDayTime();
        if (isDay)
        {
            if (ApplyConfigDuringDayTime) return configValue;
            else return unmoddedValue;
        }
        else
        {
            if (ApplyConfigDuringNightTime) return configValue;
            else return unmoddedValue;
        }
    }

    public static bool GetIsDayTime()
    {
        var dayNightCycle = DayNightCycle.main;
        if (dayNightCycle == null) return true;
        var dayScalar = dayNightCycle.GetDayScalar();
        return dayScalar > kMinDayLightScalar && dayScalar < kMaxDayLightScalar;
    }

    // no interpolation O_O
    private static float ScaleFloatWithTimeOfDay(float unmoddedValue, float configValue)
    {
        bool isDay = GetIsDayTime();
        if (isDay)
        {
            if (ApplyConfigDuringDayTime) return configValue;
            else return unmoddedValue;
        }
        else
        {
            if (ApplyConfigDuringNightTime) return configValue;
            else return unmoddedValue;
        }
    }

    private static int CalculateSearchRingScale()
    {
        var scaled = ScaleFloatWithTimeOfDay(1f, Config.Instance.DetectionRadiusMultiplier);
        return Mathf.Clamp(Mathf.RoundToInt(scaled), 0, kSearchRingScaleLimit);
    }
}
