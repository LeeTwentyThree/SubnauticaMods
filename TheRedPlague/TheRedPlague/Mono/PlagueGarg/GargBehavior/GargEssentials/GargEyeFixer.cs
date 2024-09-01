using UnityEngine;

namespace TheRedPlague.Mono.PlagueGarg.GargBehavior.GargEssentials;

class GargEyeFixer : MonoBehaviour
{
    private readonly Vector3 _overrideScale = new Vector3(0.9f, 0.9f, 0.9f);

    void LateUpdate()
    {
        transform.localScale = _overrideScale;
    }
}