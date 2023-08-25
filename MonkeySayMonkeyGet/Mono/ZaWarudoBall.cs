using UnityEngine;

namespace MonkeySayMonkeyGet.Mono;

public class ZaWarudoBall : MonoBehaviour
{
    private float realtimeStarted;

    private float lifetime = 6f;
    private float minScale = -10000f;
    private float maxScale = 100000f;

    public static void PlayVFX(Vector3 center)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.position = center;
        go.transform.localScale = Vector3.zero;
        go.AddComponent<ZaWarudoBall>();
        DestroyImmediate(go.GetComponent<Collider>());
        var renderer = go.GetComponent<Renderer>();
        go.GetComponent<MeshFilter>().mesh = Plugin.assetBundle.LoadAsset<Mesh>("InverseSphere");
        var m = renderer.material;
        m.shader = Shader.Find("MarmosetUBER");
        m.EnableKeyword("MARMO_EMISSION");
        m.SetFloat("_EnableGlow", 1f);
        m.SetFloat("_EmissionLM", 1f);
        m.SetFloat("_EmissionLMNight", 1f);
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        Destroy(go, 0.1f);
    }

    private void Start()
    {
        realtimeStarted = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        transform.localScale = Vector3.one * Mathf.Lerp(minScale, maxScale, (Time.realtimeSinceStartup - realtimeStarted) / lifetime);
    }
}
