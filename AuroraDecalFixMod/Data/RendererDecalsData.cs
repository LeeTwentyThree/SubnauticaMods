using System;
using Newtonsoft.Json;

namespace AuroraDecalFixMod.Data;

[Serializable]
public class RendererDecalsData
{
    [JsonProperty("renderer_path")]
    public string RendererPath { get; set; }
    
    [JsonProperty("decal_material_indices")]
    public int[] DecalMaterialIndices { get; set; }
}