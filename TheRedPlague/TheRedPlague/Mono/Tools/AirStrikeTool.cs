using Nautilus.Extensions;
using Nautilus.Utility;
using UnityEngine;
using TheRedPlague.Mono.AirStrikes;

namespace TheRedPlague.Mono.Tools;

public class AirStrikeTool : PlayerTool
{
    public override string animToolName => "beacon";

    private bool _throwing;
    private bool _triggered;
    
    public override void OnToolUseAnim(GUIHand hand)
    {
        // if (_throwing) return;
        // _throwing = true;
        //Invoke(nameof(Throw), 0.5f);
        Throw();
    }

    private void Start()
    {
        // pre-warm the prefab
        AirStrikeController.GetOrCreateInstance();
    }

    private void Throw()
    {
        _isInUse = false;
        
        var fpModel = GetComponent<FPModel>();
        var cameraTransform = MainCamera.camera.transform;
        pickupable.Drop(cameraTransform.position + cameraTransform.forward * 1.2f);
        transform.forward = -cameraTransform.forward;
        pickupable.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        // transform.rotation = Quaternion.LookRotation(Player.main.transform.position);
        fpModel.SetState(true);
        var canvas = fpModel.transform.SearchChild("Canvas");
        canvas.GetChild(0).gameObject.SetActive(false);
        canvas.GetChild(1).gameObject.SetActive(true);
        _throwing = false;
        
        ActivateAirStrike();
    }

    private void ActivateAirStrike()
    {
        if (_triggered) return;
        Utils.PlayFMODAsset(AudioUtils.GetFmodAsset("AirStrike"), transform.position);
        Subtitles.Add("AirStrikeSubtitles");
        _triggered = true;
        AirStrikeController.GetOrCreateInstance().AirStrikePreciseLocation(transform.position);
        Destroy(gameObject, 30);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        if (!isActiveAndEnabled) return;
        var controller = AirStrikeController.GetOrCreateInstance();
        if (controller.ExplosionEffect == null) return;
        var explodeFx = Instantiate(controller.ExplosionEffect, transform.position, Quaternion.identity);
        explodeFx.transform.localScale = Vector3.one * 0.6f;
    }
}