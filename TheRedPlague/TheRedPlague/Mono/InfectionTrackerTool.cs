using System;
using UnityEngine;

namespace TheRedPlague.Mono;

public class InfectionTrackerTool : PlayerTool
{
    public override string animToolName => "flashlight";

    public Transform arrow;
    private GameObject _model;

    private void Start()
    {
        _model = arrow.GetChild(0).gameObject;
    }

    private void LateUpdate()
    {
        var hideArrow = Player.main.IsSwimming() && Player.main.rigidBody.velocity.sqrMagnitude > 0.01f;
        var plagueHeart = PlagueHeartBehavior.main;
        if (hideArrow || plagueHeart == null)
        {
            _model.SetActive(false);
            return;
        }
        _model.SetActive(true);
        arrow.LookAt(plagueHeart.transform);
    }
}