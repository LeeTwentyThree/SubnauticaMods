using UnityEngine;

namespace PdaUpgradeCards.MonoBehaviours.UI;

public class LeviathanDetectorUI : MonoBehaviour, IManagedUpdateBehaviour
{
    private const float ScaleVariation = 0.1f;
    private const float PulseDuration = 1f;

    public static LeviathanDetectorUI main;

    private void OnEnable()
    {
        BehaviourUpdateUtils.Register(this);
    }

    private void OnDisable()
    {
        BehaviourUpdateUtils.Deregister(this);
    }

    public string GetProfileTag()
    {
        return "LeviathanDetectorUI";
    }

    public void ManagedUpdate()
    {
        transform.localScale = Vector3.one *
                               (1 + Mathf.PingPong(Time.time, PulseDuration) * ScaleVariation * 2f - ScaleVariation);
    }

    public int managedUpdateIndex { get; set; }
}