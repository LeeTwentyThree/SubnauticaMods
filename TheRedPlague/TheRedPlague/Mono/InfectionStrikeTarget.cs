using System.Collections.Generic;
using UnityEngine;

namespace TheRedPlague.Mono;

public class InfectionStrikeTarget : MonoBehaviour
{
    public static readonly List<InfectionStrikeTarget> AllTargets = new List<InfectionStrikeTarget>();
    
    private InfectedMixin _infectedMixin;

    private void Start()
    {
        _infectedMixin = GetComponent<InfectedMixin>();
    }

    public bool IsValidTarget()
    {
        if (_infectedMixin == null) return false;
        return !_infectedMixin.IsInfected();
    }

    public void Infect()
    {
        if (_infectedMixin)
        {
            _infectedMixin.SetInfectedAmount(4);
        }
    }

    private void OnEnable()
    {
        AllTargets.Add(this);
    }
    
    private void OnDisable()
    {
        AllTargets.Remove(this);
    }
}