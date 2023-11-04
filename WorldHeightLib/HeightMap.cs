using UnityEngine;

namespace WorldHeightLib;

public class HeightMap : MapBase<float>
{
    public static HeightMap Instance { get; internal set; }

    private readonly int _quality;
    private readonly float _heightOffset;
    
    // Quality should be a power of 10. Determines decimal place accuracy.
    internal HeightMap(Texture2D texture, int downscaledResolution, int quality, float mapScale, float heightOffset) : base(texture, downscaledResolution, mapScale)
    {
        _quality = quality;
        _heightOffset = heightOffset;
    }

    protected override float GetValueFromPixel(Color32 color)
    {
        return NumberFromRGBColor(color, _quality) + _heightOffset;
    }
    
    private static float NumberFromRGBColor(Color32 input, int quality)
    {
        return (float)System.BitConverter.ToInt32(new byte[] { 0x00, input.r, input.g, input.b }, 0) / quality / 256;
    }
}