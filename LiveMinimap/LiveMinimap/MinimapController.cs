using UnityEngine;
using UnityEngine.UI;

namespace LiveMinimap;

internal class MinimapController : MonoBehaviour
{
    private static MinimapController main;
    private Camera camera;
    private RawImage image;
    private RenderTexture renderTexture;

    public static MinimapController Main
    {
        get
        {
            if (main == null)
                return Create();
            return main;
        }
    }

    private static MinimapController Create()
    {
        var go = new GameObject("MinimapController");
        var c = go.AddComponent<MinimapController>();
        c.camera = go.AddComponent<Camera>();
        go.transform.forward = Vector3.down;
        c.renderTexture = new RenderTexture(256, 256, 0);
        c.camera.targetTexture = c.renderTexture;
        c.camera.cullingMask = LayerMask.GetMask("Default");
        c.camera.fieldOfView = Plugin.config.FOV;

        var canvas = Instantiate(Plugin.bundle.LoadAsset<GameObject>("MinimapCanvas"));
        canvas.transform.parent = go.transform;
        c.image = canvas.transform.Find("Content/Mask/MapDisplay").GetComponent<RawImage>();
        c.image.texture = c.renderTexture;

        return c;
    }

    private void OnDestroy()
    {
        renderTexture.Release();
        Destroy(renderTexture);
    }
    
    private void Update()
    {
        camera.fieldOfView = Plugin.config.FOV;
        transform.eulerAngles = new Vector3(90, MainCamera.camera.transform.eulerAngles.y, 0);
        var distAbove = Plugin.config.Height;
        if (Physics.Raycast(new Ray(MainCamera.camera.transform.position, Vector3.up), out var hit, Plugin.config.Height, 1 << LayerID.TerrainCollider, QueryTriggerInteraction.Ignore))
        {
            distAbove = hit.distance;
        }
        transform.position = MainCamera.camera.transform.position + Vector3.up * Plugin.config.Height;
        camera.nearClipPlane = Plugin.config.Height - distAbove + 0.1f;
        camera.farClipPlane = 200f;
    }
}