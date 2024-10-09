using Nautilus.Utility;
using TheRedPlague.PrefabFiles.Resources;
using UnityEngine;

namespace TheRedPlague.Mono.Tools;

public class TransfuserTool : PlayerTool
{
    public override string animToolName => "transfuser";

    private static readonly FMODAsset FailSound = AudioUtils.GetFmodAsset("event:/tools/transfuser/fail");
    private static readonly FMODAsset TakeSampleSound = AudioUtils.GetFmodAsset("event:/tools/transfuser/take_sample");

    private bool _busy;

    private float _lastFailTime = -100;
    
    public override string GetCustomUseText()
    {
        if (_busy) return Language.main.Get("UseInfectionSamplerBusy");
        if (Time.time < _lastFailTime + 2) return Language.main.Get("UseInfectionSamplerFail");
        return LanguageCache.GetButtonFormat("UseInfectionSampler", GameInput.Button.RightHand);
    }
    
    public override bool OnRightHandDown()
    {
        if (_busy) return false;
        if (Targeting.GetTarget(Player.main.gameObject, 3, out var target, out var distance))
        {
            var infected = target.GetComponent<InfectedMixin>();
            if (infected != null)
            {
                var isInfected = infected.IsInfected();
                if (isInfected)
                {
                    Utils.PlayFMODAsset(TakeSampleSound, transform.position);
                    Invoke(nameof(AddSampleDelayed), 6);
                    _busy = true;
                    return true;
                }
            }
        }
        Utils.PlayFMODAsset(FailSound, transform.position);
        _lastFailTime = Time.time;
        return true;
    }

    private void AddSampleDelayed()
    {
        _busy = false;
        CraftData.AddToInventory(RedPlagueSample.Info.TechType);
        StoryUtils.TransfuserSampleTakenEvent.Trigger();
    }
}