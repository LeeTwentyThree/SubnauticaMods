using System.Collections.Generic;
using PdaUpgradeCards.MonoBehaviours.Upgrades;
using UnityEngine;

namespace PdaUpgradeCards.MonoBehaviours;

public class PocketDimensionSub : SubRoot, IScheduledUpdateBehaviour
{
    private const float KickPlayerOutDistance = 50f;
    
    public TechType dimensionTechType;
    
    public Transform entrancePosition;
    
    private static readonly Dictionary<TechType, PocketDimensionSub> PocketDimensionSubs = new();

    public override void Awake()
    {
        base.Awake();
        PocketDimensionSubs.Add(dimensionTechType, this);
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
    }

    private void OnEnable()
    {
        UpdateSchedulerUtils.Register(this);
    }
    
    private void OnDisable()
    {
        UpdateSchedulerUtils.Deregister(this);
    }

    public static bool TryGetPocketDimension(TechType type, out PocketDimensionSub dimension)
    {
        return PocketDimensionSubs.TryGetValue(type, out dimension);
    }

    public string GetProfileTag()
    {
        return "PocketDimensionSub";
    }

    public void ScheduledUpdate()
    {
        if (Player.main.GetCurrentSub() != this)
            return;
        
        PocketDimensionUpgrade.QueryKickOutPlayer(this);
        
        if (Vector3.SqrMagnitude(Player.main.transform.position - transform.position) >
            KickPlayerOutDistance * KickPlayerOutDistance)
        {
            Player.main.SetCurrentSub(null);
        }
    }

    public int scheduledUpdateIndex { get; set; }
}