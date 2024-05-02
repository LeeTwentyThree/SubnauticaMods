using UnityEngine;

namespace WorldHeightLib;

public class NormalMap : MapBase<Vector3>
{
    public static NormalMap Instance { get; internal set; }
    
    internal NormalMap(Texture2D texture, int downscaledResolution, float mapScale) : base(texture, downscaledResolution, mapScale)
    {
    }

    protected override Vector3 GetValueFromPixel(Color32 color)
    {
        return new Vector3((color.r / 255f - 0.5f) * 2, (color.b / 255f - 0.5f) * 2, (color.g / 255f - 0.5f) * 2);
    }
}