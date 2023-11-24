using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace WeatherMod;

public static class Extensions
{
    public static T GetRandomUnity<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
}