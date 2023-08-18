using Nautilus.Json;
using Nautilus.Json.Attributes;
using System;

namespace CustomWaterLevel;

[FileName("WaterLevelData")]
[Serializable]
public class WaterLevelData : SaveDataCache
{
    public float WaterLevel;
    public float TimeLastChange;
}