using System.Collections.Generic;
using Nautilus.Json;
using Nautilus.Json.Attributes;

namespace SeaVoyager;

[FileName("Docks")]
public class DockSaveData : SaveDataCache
{
    public Dictionary<string, float> savedRotations;
    public Dictionary<string, float> savedCableLocations;
}