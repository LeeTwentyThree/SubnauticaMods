using System;
using Newtonsoft.Json;

namespace AuroraDecalFixMod.Data;

[Serializable]
public class AllDecalData
{
    [JsonProperty("prefabs")]
    public PrefabDecalData[] Prefabs { get; set; }
}