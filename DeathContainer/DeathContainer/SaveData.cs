using Nautilus.Handlers;
using Nautilus.Json;
using System.Collections.Generic;
using UnityEngine;

namespace DeathContainer;

internal class SaveData : SaveDataCache
{
    public Dictionary<string, SaveContainer> saveData;
    public int deaths;

    public static SaveData Main { get; } = SaveDataHandler.RegisterSaveDataCache<SaveData>();
}

internal class SaveContainer
{
    public Vector3 coords;
    public int deathNumber;
}