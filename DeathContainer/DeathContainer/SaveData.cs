using Nautilus.Handlers;
using Nautilus.Json;
using System.Collections.Generic;
using UnityEngine;

namespace DeathContainer;

internal class SaveData : SaveDataCache
{
    public Dictionary<string, SaveContainer> graves = new Dictionary<string, SaveContainer>();
    public List<string> obtainedGraves = new List<string>();
    public int deaths;

    public static SaveData main;
}

[System.Serializable]
internal class SaveContainer
{
    public Vector3 coords;
    public int deathNumber;
    public string deathInfo;
    public int width;
    public int height;

    public SaveContainer(Vector3 coords, int deathNumber, string deathInfo, int width, int height)
    {
        this.coords = coords;
        this.deathNumber = deathNumber;
        this.deathInfo = deathInfo;
        this.width = width;
        this.height = height;
    }
}