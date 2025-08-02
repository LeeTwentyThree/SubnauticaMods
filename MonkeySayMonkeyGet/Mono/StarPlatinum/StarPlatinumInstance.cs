using UnityEngine;
using UWE;
using System.Collections;
using System.Collections.Generic;

namespace MonkeySayMonkeyGet.Mono.StarPlatinum;

public class StarPlatinumInstance : MonoBehaviour
{
    public Animator animator;
    public List<GameObject> armVfx;

    public static IEnumerator CreateInstance(IOut<StarPlatinumInstance> result)
    {
        var spawnedPrefab = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("StarPlatinumPrefab"));
        spawnedPrefab.SetActive(false);
        spawnedPrefab.transform.localScale = Vector3.one * 4f;
        var instance = spawnedPrefab.AddComponent<StarPlatinumInstance>();
        instance.animator = instance.gameObject.GetComponentInChildren<Animator>();
        instance.animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        var modelParent = instance.transform.GetChild(0);
        instance.armVfx = new List<GameObject>() { modelParent.GetChild(1).gameObject, modelParent.GetChild(2).gameObject, modelParent.GetChild(1).gameObject, modelParent.GetChild(1).gameObject };

        Utils.ApplySNShaders(instance.gameObject, 8f, 1f, 3f);
        instance.gameObject.AddComponent<SkyApplier>().renderers = instance.gameObject.GetComponentsInChildren<Renderer>();

        var task = PrefabDatabase.GetPrefabAsync("14bbf7f0-4276-48bf-868b-317b366edd16");

        yield return task;

        if (task.TryGetPrefab(out var firePrefab))
        {
            var fire = Instantiate(firePrefab);
            Utils.RemovePrefabComponents(fire);

            var fireComponent = fire.GetComponentInChildren<Fire>(true);
            var fireFx = Instantiate(fireComponent.fireFXprefab);
            fireFx.transform.parent = fire.transform;
            Destroy(fireComponent);

            fireFx.transform.parent = spawnedPrefab.transform;
            fireFx.transform.localPosition = new Vector3(0f, 0.5f, 0f);
            fireFx.transform.localScale = Vector3.one * 0.6f;
            var extinguishableFire = fireFx.GetComponentInChildren<VFXExtinguishableFire>();
            Destroy(extinguishableFire);
            foreach (var renderer in fireFx.GetComponentsInChildren<Renderer>())
            {
                var color = new Color(1f, 1f, 4f);
                if (renderer.gameObject.name == "x_SmokeLight_Cylindrical")
                {
                    color = new Color(0.5f, 0.5f, 1f);
                }
                if (renderer.material) renderer.material.SetColor(ShaderPropertyID._Color, color);
            }
            fireFx.transform.GetChild(0).gameObject.SetActive(false);
            var crossPlanes = fireFx.transform.GetChild(4);
            crossPlanes.localScale = new Vector3(0.17f, 0.4f, 0.17f);
            crossPlanes.GetComponent<Renderer>().material.color = new Color(0.24f, 0f, 1f);
            fireFx.transform.GetChild(5).gameObject.SetActive(false);
        }

        spawnedPrefab.SetActive(true);
        result.Set(instance);
    }

    public void MoveToPlayer()
    {
        var mainCam = MainCameraControl.main.transform;
        transform.position = mainCam.position + mainCam.transform.right * 3f + mainCam.transform.forward * 7f;
        var forward = mainCam.transform.forward;
        forward = new Vector3(forward.x, 0f, forward.z).normalized;
        transform.forward = forward;

        Utils.PlaySoundEffect("StarPlatinumSummon", 1f);
    }

    public T AssignTask<T>() where T : OraSequence
    {
        var existing = gameObject.GetComponent<OraSequence>();
        if (existing) return null;
        var component = gameObject.AddComponent<T>();
        component.animator = animator;
        component.armVFX = armVfx;
        return component;
    }

    public void CancelAllTasks()
    {
        foreach (var task in gameObject.GetComponents<OraSequence>())
        {
            task.EndSequence();
        }
    }

    public void DestroyInstance()
    {
        Destroy(gameObject);
        Utils.PlaySoundEffect("StarPlatinumSummon", 1f);
    }

    private void Update()
    {
        if (!VoiceCommands.StarPlatinum.StarPlatinumActivated && gameObject.GetComponent<OraSequence>() == null)
        {
            DestroyInstance();
        }
    }
}
