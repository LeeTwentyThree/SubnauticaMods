using System.Collections.Generic;
using UnityEngine;

namespace PdaUpgradeCards.MonoBehaviours;

public class PocketDimensionSub : SubRoot
{
    public TechType dimensionTechType;
    
    public Transform entrancePosition;
    
    private static Dictionary<TechType, PocketDimensionSub> PocketDimensionSubs = new();

    public override void Awake()
    {
        base.Awake();
        PocketDimensionSubs.Add(dimensionTechType, this);
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
    }

    public static bool TryGetPocketDimension(TechType type, out PocketDimensionSub dimension)
    {
        return PocketDimensionSubs.TryGetValue(type, out dimension);
    }
}