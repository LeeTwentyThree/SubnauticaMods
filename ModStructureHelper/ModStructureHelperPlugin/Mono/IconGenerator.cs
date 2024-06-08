using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace ModStructureHelperPlugin.Mono;

public class IconGenerator : MonoBehaviour
{
    public Camera cam;
    public GameObject sceneParent;
    public RenderTexture renderTexture;

    private static IconGenerator _instance;

    private static Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();

    public static bool TryGetIcon(string classId, out Sprite sprite)
    {
        if (_sprites.TryGetValue(classId, out sprite)) return true;
        return false;
    }

    public static bool HasIcon(string classId) => _sprites.ContainsKey(classId);
    
    public static IEnumerator GenerateIcon(GameObject prefab, string classId, IconOutput output)
    {
        if (_instance == null)
        {
            var obj = new GameObject("Icon Generator");
            _instance = obj.AddComponent<IconGenerator>();
            var camObj = new GameObject("Icon generator camera");
            var camera = camObj.AddComponent<Camera>();
            camera.enabled = false;
            var renderTexture = new RenderTexture(256, 256, 0, RenderTextureFormat.Default);
            _instance.renderTexture = renderTexture;
            camera.targetTexture = renderTexture;
            var sceneParent = new GameObject("Scene parent");
            camObj.transform.parent = sceneParent.transform;
            camObj.transform.localEulerAngles = new Vector3(0, 210, 0);
            _instance.sceneParent = sceneParent;
            _instance.cam = camera;
            _instance.sceneParent.transform.parent = _instance.transform;
            _instance.transform.localPosition = new Vector3(0, 2000, 0);
        }

        if (_sprites.TryGetValue(classId, out var icon))
        {
            output.Sprite = icon;
            yield break;
        }

        _instance.cam.enabled = true;
        _instance.sceneParent.SetActive(true);
        var model = Instantiate(prefab);
        model.SetActive(true);
        model.transform.position = _instance.transform.position;
        DestroyImmediate(model.GetComponent<LargeWorldEntity>());
        var bounds = GetObjectBounds(model);
        _instance.cam.transform.position = model.transform.position + Vector3.ClampMagnitude(bounds.extents, 30) + new Vector3(1.5f, -0.5f, 2);
        yield return null;
        RenderTexture.active = _instance.renderTexture;
        Texture2D tex = new Texture2D(256, 256, TextureFormat.RGBA32, false, true);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0, false);
        tex.Apply(false);
        _instance.sceneParent.SetActive(false);
        Destroy(model);
        var sprite = Sprite.Create(tex, new Rect(0, 0, 256, 256), new Vector2(0.5f, 0.5f));
        _instance.cam.enabled = false;
        _sprites[classId] = sprite;
        output.Sprite = sprite;
    }

    private static Bounds GetObjectBounds(GameObject obj)
    {
        Bounds bounds = new Bounds(obj.transform.position, Vector3.one * 1f);
        foreach (var renderer in obj.GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(renderer.bounds);
        }
        return bounds;
    }

    public class IconOutput
    {
        public Sprite Sprite { get; set; }
    }
}