using System.IO;
using UnityEngine;

namespace CustomWaterLevel;

internal class WaterMove : MonoBehaviour
{
    public static WaterMove main;

    public float waterLevel;

    private WaterLevelData save;
    private bool firstLoad;
    private float timeSaveAgain;
    private float targetWaterLevel;
    private bool waterIsMoving;

    private const float saveDelay = 5f;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        save = new WaterLevelData();
        LoadData();
    }

    private void LoadData()
    {
        if (!File.Exists(save.JsonFilePath))
        {
            firstLoad = true;
        }
        save.Load();
        if (firstLoad)
        {
            save.WaterLevel = Plugin.config.WaterLevel;
            save.TimeLastChange = 0f;
        }
        waterLevel = save.WaterLevel;
    }

    public bool InUse
    {
        get
        {
            return Plugin.config.AutomaticChange;
        }
    }

    private void Update()
    {
        CalculateWaterLevel();
        if (DayNightCycle.main.timePassedSinceOrigin > save.TimeLastChange + Plugin.config.IntervalDuration)
        {
            targetWaterLevel = waterLevel + Plugin.config.IntervalChange;
            save.TimeLastChange = DayNightCycle.main.timePassedSinceOrigin;
            waterIsMoving = true;
        }
        if (Time.time > timeSaveAgain)
        {
            SaveData();
            timeSaveAgain = Time.time + saveDelay;
        }
    }

    private void SaveData()
    {
        save.WaterLevel = waterLevel;
        save.Save();
    }

    private void CalculateWaterLevel()
    {
        if (save == null)
        {
            ErrorMessage.AddMessage("Error: No WaterLevelData present!");
            return;
        }
        if (waterIsMoving)
        {
            waterLevel = Mathf.MoveTowards(waterLevel, targetWaterLevel, Time.deltaTime * Plugin.config.WaterMoveSpeed / 4f);
            if (Mathf.Approximately(waterLevel, targetWaterLevel))
            {
                waterIsMoving = false;
            }
        }
    }
}