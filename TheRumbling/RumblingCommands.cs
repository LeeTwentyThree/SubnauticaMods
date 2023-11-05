using Nautilus.Commands;
using UnityEngine;
using WorldHeightLib;

namespace TheRumbling;

public static class RumblingCommands
{
    [ConsoleCommand("rumbling")]
    public static void BeginRumbling()
    {
        RumblingManager.BeginRumblingEvent();
    }
    
    [ConsoleCommand("killtitans")]
    public static void KillTitans()
    {
        RumblingManager.KillAll();
    }
    
    [ConsoleCommand("deletetitans")]
    public static void DeleteTitans()
    {
        RumblingManager.DeleteAll();
    }
    
    [ConsoleCommand("titanemote")]
    public static void TitanEmote(string emoteName = null)
    {
        RumblingManager.EmoteAll(emoteName);
    }
    
    [ConsoleCommand("debugheightmap")]
    public static void DebugHeightmap(int distance, int step)
    {
        var center = MainCamera.camera.transform.position;
        for (var x = center.x - distance; x < center.x + distance; x += step)
        {
            for (var z = center.z - distance; z < center.z + distance; z += step)
            {
                if (HeightMap.Instance.TryGetValueAtPosition(new Vector2(x, z), out var value))
                {
                    var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(x, value, z);
                }
            }
        }
    }
}