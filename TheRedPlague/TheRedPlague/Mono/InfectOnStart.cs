using System;
using UnityEngine;

namespace TheRedPlague.Mono;

public class InfectOnStart : MonoBehaviour
{
    private void Start()
    {
        var infection = GetComponent<InfectedMixin>();
        if (infection.IsInfected())
            return;
        infection.SetInfectedAmount(4);
    }
}