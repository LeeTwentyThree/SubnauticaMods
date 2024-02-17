using UnityEngine;

namespace TheRedPlague.Mono;

public class FixMassiveCreatures : MonoBehaviour
{
    public float desiredLossyScale;

    private void Update()
    {
        if (transform.lossyScale.x > desiredLossyScale)
        {
            var parentScale = GetParentScale();
            transform.localScale = Vector3.one * (desiredLossyScale / parentScale);
        }
    }

    private float GetParentScale()
    {
        if (transform.parent == null) return 1;
        return transform.parent.lossyScale.magnitude;
    }
}