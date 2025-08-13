using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using UnityEngine;
using DeathContainer.Mono;
using TMPro;

namespace DeathContainer.Items.Prefabs;
public static class DeathContainerPrefab
{
    public static PrefabInfo Info { get; } = PrefabInfo
        .WithTechType("DeathContainer", "Death Container", "Stores any items lost on death.")
        .WithIcon(Plugin.AssetBundle.LoadAsset<Sprite>("DeathContainerIcon"));

    public static void Register()
    {
        var customPrefab = new CustomPrefab(Info);

        var obj = new CloneTemplate(Info, TechType.SmallStorage);
        obj.ModifyPrefab += (GameObject obj) =>
        {
            obj.transform.Find("1st_person_model").gameObject.SetActive(false);
            obj.transform.Find("3rd_person_model").gameObject.SetActive(true);
            var container = obj.GetComponentInChildren<StorageContainer>();
            obj.AddComponent<DeathContainerBehaviour>();
            obj.GetComponentInChildren<uGUI_SignInput>().enabled = false;
            obj.GetComponentInChildren<TMP_InputField>().enabled = false;
            obj.GetComponentInChildren<ColoredLabel>().enabled = false;
            obj.transform.Find("LidLabel/Label").GetComponent<Collider>().enabled = false;
            obj.transform.Find("LidLabel/Label/UI").gameObject.SetActive(true);
            var text = obj.GetComponentInChildren<TextMeshProUGUI>();
            text.color = Color.red;
            text.fontStyle = FontStyles.Bold | FontStyles.Underline;
            text.text = "DEATH";
            text.fontSize = 45;
            foreach (var renderer in obj.transform.Find("3rd_person_model").gameObject.GetComponentsInChildren<Renderer>())
            {
                foreach (var material in renderer.materials)
                {
                    material.SetColor("_GlowColor", new Color(10, 0, 0));
                }
            }
            var light = obj.AddComponent<Light>();
            light.color = Color.red;
            light.intensity = 1;
            light.range = 7;
            var ping = obj.EnsureComponent<PingInstance>();
            ping.origin = obj.transform;
            ping.SetType(Plugin.DeathContainerPingType);
            ping.visitDistance = 1f;
            ping.minDist = 1f;
            ping.range = 5f;
            obj.AddComponent<DestroyContainerIfEmpty>().container = container;
        };
        customPrefab.SetGameObject(obj);
        customPrefab.Register();
    }
}