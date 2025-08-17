using System;
using Newtonsoft.Json;

namespace AuroraDecalFixMod.Data;

[Serializable]
public class PrefabDecalData
{
    [JsonProperty("class_id")]
    public string ClassId { get; set; }
    
    [JsonProperty("renderers")]
    public RendererDecalsData[] Renderers { get; set; }
}