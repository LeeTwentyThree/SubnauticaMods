using System;
using UnityEngine;

namespace TheRedPlague.Mono;

public class NpcSurvivorManager : MonoBehaviour
{
    public static NpcSurvivorManager main;

    private void Awake()
    {
        main = this;
    }
}