using UnityEngine;

namespace PodshellLeviathan.Mono;

public class PodshellBabyHeadScaler : MonoBehaviour
{
    public Transform headTransform;
    public float scale = 1.3f;

    private Vector3 _scale;

    private void Start()
    {
        _scale = Vector3.one * scale;
    }
    
    private void LateUpdate()
    {
        headTransform.localScale = _scale;
    }
}