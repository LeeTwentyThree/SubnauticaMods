using UnityEngine;

namespace WorldHeightLib;

public abstract class MapBase<T>
{
    private T[,] _values;
    private readonly Texture2D _texture;
    private readonly int _downscaledResolution;
    private readonly float _mapScale;
    private float MapExtents => _mapScale / 2f;

    protected MapBase(Texture2D texture, int downscaledResolution, float mapScale)
    {
        _texture = texture;
        _downscaledResolution = downscaledResolution;
        _mapScale = mapScale;
    }

    public void ProcessMapData()
    {
        var heightmapTextureOriginalResolution = _texture.GetSize().x;
        var downscalingFactor = heightmapTextureOriginalResolution / _downscaledResolution;

        _values = new T[_downscaledResolution, _downscaledResolution];

        for (var x = 0; x < _downscaledResolution; x++)
        {
            for (var y = 0; y < _downscaledResolution; y++)
            {
                var color = _texture.GetPixel(x * downscalingFactor, y * downscalingFactor);
                _values[x, y] = GetValueFromPixel(color);
            }
        }
        
        Plugin.Logger.LogInfo($"Processed {ToString()} map!");
    }

    public bool TryGetValueAtPosition(Vector2 horizontalPosition, out T value)
    {
        var index = MapPositionToIndex(horizontalPosition);
        if (index.x == -1 || index.y == -1)
        {
            value = default;
            return false;
        }

        value = _values[index.x, index.y];
        return true;
    }

    // Returns the index or -1, -1 if out of range
    private Vector2Int MapPositionToIndex(Vector2 horizontalPosition)
    {
        var index = new Vector2Int(
            (int)Utils.Remap(horizontalPosition.x, -MapExtents, MapExtents, 0, _downscaledResolution),
            (int)Utils.Remap(horizontalPosition.y, -MapExtents, MapExtents, 0, _downscaledResolution));
        
        if (index.x < 0 || index.y < 0 || index.x >= _downscaledResolution || index.y >= _downscaledResolution)
            return new Vector2Int(-1, -1);

        return index;
    }
    
    protected abstract T GetValueFromPixel(Color32 color);
}