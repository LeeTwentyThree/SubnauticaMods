using System;
using UnityEngine;

namespace TheRedPlague.Mono;

public class SetBeaconColor : MonoBehaviour
{
    public int colorIndex;
    
    private void Start()
    {
        GetComponent<PingInstance>().SetColor(colorIndex);
    }
}