using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Handlers;
using UnityEngine;

namespace KallieʼsPropPack.Prefabs.Plants;

public class KelpRoot
{
    private PrefabInfo Info { get; }
    private string OriginalClassId { get; }

    public KelpRoot(string classId, string originalClassId)
    {
        Info = PrefabInfo.WithTechType(classId);
        OriginalClassId = originalClassId;
    }

    public void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(new CloneTemplate(Info, OriginalClassId)
        {
            ModifyPrefab = obj =>
            {
                foreach (var light in obj.GetComponentsInChildren<Light>())
                {
                    light.color = new Color(0.95f, 0.85f, 0);
                }

                foreach (Transform child in obj.transform)
                {
                    if (child.gameObject.name is "model" or "models")
                    {
                        foreach (var renderer in child.gameObject.GetComponentsInChildren<Renderer>())
                        {
                            var material = renderer.material;
                            material.color = new Color(0.2f, 0.62f, 0.25f);
                            material.SetColor(ShaderPropertyID._SpecColor, new Color(0, 0.71f, 0.38f));
                            material.SetColor(ShaderPropertyID._GlowColor, new Color(1, 4f, 0f));
                            material.SetFloat(ShaderPropertyID._EmissionLM, 0);
                            material.SetFloat("_EmissionLMNight", 0);
                            material.SetFloat("_Shininess", 5);
                            material.SetFloat("_SpecInt", 0.2f);
                        }

                        continue;
                    }

                    foreach (var renderer in child.gameObject.GetComponentsInChildren<Renderer>())
                    {
                        var material = renderer.material;
                        material.color = new Color(1, 0.905f, 0f);
                        material.SetColor(ShaderPropertyID._SpecColor, new Color(2.5f, 1.8f, 0));
                        material.SetColor(ShaderPropertyID._GlowColor, new Color(0.84f, 1.6f, 0));
                        material.SetFloat("_SpecInt", 0.6f);
                        material.SetFloat("_Shininess", 4.2f);
                        material.SetFloat("_Fresnel", 0.42f);
                    }
                }
            }
        });
        prefab.Register();
        WorldEntityDatabaseHandler.AddCustomInfo(Info.ClassID, Info.TechType, Vector3.one, true,
            LargeWorldEntity.CellLevel.Medium, EntitySlot.Type.Large);
    }
}