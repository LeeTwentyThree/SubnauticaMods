using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheRedPlague.Mono;

public class InfectionTarget : MonoBehaviour
{
    private static readonly List<InfectionTarget> AllTargets = new();

    public bool invalidTarget;
    
    private InfectedMixin _infectedMixin;

    private void Start()
    {
        _infectedMixin = GetComponent<InfectedMixin>();
    }

    public bool IsValidTarget()
    {
        if (invalidTarget) return false;
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
    
    public static bool TryGetRandomTarget(out InfectionTarget chosenTarget)
    {
        var validTargets = AllTargets.Where(target => target.IsValidTarget()).ToArray();

        if (validTargets.Length == 0)
        {
            chosenTarget = null;
            return false;
        }

        chosenTarget = validTargets[Random.Range(0, validTargets.Length)];
        return chosenTarget != null;
    }
}