using UnityEngine;

namespace AggressiveFauna;

internal class ApocalypseWarning : MonoBehaviour
{
    private bool ShouldWarnAtAll
    {
        get
        {
            return ShouldWarnForDaytime != ShouldWarnForNighttime;
        }
    }

    private bool ShouldWarnForDaytime
    {
        get
        {
            return AggressionSettings.ApplyConfigDuringDayTime;
        }
    }

    private bool ShouldWarnForNighttime
    {
        get
        {
            return AggressionSettings.ApplyConfigDuringNightTime;
        }
    }

    private bool isDayTime;

    private void Start()
    {
        isDayTime = AggressionSettings.GetIsDayTime();
    }

    private void Update()
    {
        bool wasDayTime = isDayTime;
        isDayTime = AggressionSettings.GetIsDayTime();
        if (!ShouldWarnAtAll) return;
        bool justBecameNight = wasDayTime && !isDayTime;
        bool justBecameDay = !wasDayTime && isDayTime;
        bool aggressionWarnings = Config.Instance.ShowAggressionWarnings;
        bool change = false;
        if (ShouldWarnForDaytime && justBecameDay)
        {
            if (aggressionWarnings) ErrorMessage.AddMessage("Increased aggression levels detected...");
            change = true;
        }
        else if (ShouldWarnForNighttime && justBecameNight)
        {
            if (aggressionWarnings) ErrorMessage.AddMessage("The smell of blood fills the water as the sun falls beyond the horizon...");
            change = true;
        }
        if (change && Config.Instance.PlayAggressionMusic) ApocalypseMusic.Play();
    }
}
