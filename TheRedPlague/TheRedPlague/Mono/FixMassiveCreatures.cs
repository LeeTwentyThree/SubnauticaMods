using UnityEngine;

namespace TheRedPlague.Mono;

public class FixMassiveCreatures : MonoBehaviour
{
    public float desiredLossyScale;

    private void Start()
    {
        var parentScale = GetParentScale();
        if (transform.lossyScale.magnitude > desiredLossyScale)
        {
            transform.localScale = Vector3.one * 1 / parentScale;
        }
    }

    private float GetParentScale()
    {
        if (transform.parent == null) return 1;
        return transform.parent.lossyScale.magnitude;
    }
}