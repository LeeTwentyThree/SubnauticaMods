using System.Collections.Generic;
using PdaUpgradeChips.MonoBehaviours.Upgrades;
using UnityEngine;

namespace PdaUpgradeChips.MonoBehaviours;

public class PocketDimensionSub : SubRoot, IScheduledUpdateBehaviour
{
    private const float KickPlayerOutDistance = 50f;
    
    public TechType dimensionTechType;
    
    public Transform entrancePosition;

    public float entranceCamRotation;
    
    private static readonly Dictionary<TechType, PocketDimensionSub> PocketDimensionSubs = new();

    private CanvasGroup _pingsCanvasGroup;

    private bool _playerWasInside;

    public override void Awake()
    {
        base.Awake();
        PocketDimensionSubs.Add(dimensionTechType, this);
        var ugui = uGUI.main;
        if (ugui)
        {
            var pingsCanvas = ugui.transform.Find("ScreenCanvas/Pings");
            if (pingsCanvas)
            {
                _pingsCanvasGroup = pingsCanvas.gameObject.GetComponent<CanvasGroup>();
            }
        }
    }

    private new void OnDestroy()
    {
        if (PocketDimensionSubs.TryGetValue(dimensionTechType, out var sub) && sub == this)
            PocketDimensionSubs.Remove(dimensionTechType);
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
        {
            if (_playerWasInside)
            {
                if (_pingsCanvasGroup)
                    _pingsCanvasGroup.alpha = 1;
                _playerWasInside = false;
            }
            return;
        }
        if (!_playerWasInside)
        {
            if (_pingsCanvasGroup)
                _pingsCanvasGroup.alpha = 0;
            _playerWasInside = true;
        }
        
        PocketDimensionUpgrade.QueryKickOutPlayer(this);
        
        if (Vector3.SqrMagnitude(Player.main.transform.position - transform.position) >
            KickPlayerOutDistance * KickPlayerOutDistance)
        {
            Player.main.SetCurrentSub(null);
        }
    }

    public int scheduledUpdateIndex { get; set; }
}