using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using FMOD;
using FMODUnity;

namespace ShipMod
{
    public static class Helpers
    {
        public static GameObject FindChild(GameObject parent, string byName)
        {
            foreach (Transform child in parent.transform)
            {
                if (child.gameObject.name == byName)
                {
                    return child.gameObject;
                }
                GameObject recursive = FindChild(child.gameObject, byName);
                if(recursive)
                {
                    return recursive;
                }
            }
            return null;
        }

        public static FMODAsset GetFmodAsset(string path)
        {
            var asset = ScriptableObject.CreateInstance<FMODAsset>();
            asset.path = path;
            return asset;
        }

        //I know this is horribly messy, I don't know what half the properties here do, but it works.
        public static void ApplyMaterials(GameObject ontoObject)
        {
            var renderers = ontoObject.GetComponentsInChildren<Renderer>();
            var shader = Shader.Find("MarmosetUBER");

            foreach (var renderer in renderers)
            {
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    Material mat = renderer.materials[i];
                    mat.shader = shader;
                    mat.SetFloat("_Glossiness", 0.6f);
                    Texture specularTexture = mat.GetTexture("_SpecGlossMap");
                    if (specularTexture != null)
                    {
                        mat.SetTexture("_SpecTex", specularTexture);
                        mat.SetFloat("_SpecInt", 1f);
                        mat.SetFloat("_Shininess", 3f);
                        mat.EnableKeyword("MARMO_SPECMAP");
                        mat.SetColor("_SpecColor", new Color(0.796875f, 0.796875f, 0.796875f, 0.796875f));
                        mat.SetFloat("_Fresnel", 0f);
                        mat.SetVector("_SpecTex_ST", new Vector4(1.0f, 1.0f, 0.0f, 0.0f));
                    }

                    if (mat.GetTexture("_BumpMap"))
                    {
                        mat.EnableKeyword("_NORMALMAP");
                    }
                    if (mat.name.ToLower().StartsWith("decal"))
                    {
                        mat.EnableKeyword("MARMO_ALPHA_CLIP");
                    }
                    if (mat.name.ToLower().StartsWith("fade"))
                    {
                        mat.EnableKeyword("_ZWRITE_ON");
                        mat.EnableKeyword("WBOIT");
                        mat.SetInt("_ZWrite", 0);
                        mat.SetInt("_Cutoff", 0);
                        mat.SetFloat("_SrcBlend", 1f);
                        mat.SetFloat("_DstBlend", 1f);
                        mat.SetFloat("_SrcBlend2", 0f);
                        mat.SetFloat("_DstBlend2", 10f);
                        mat.SetFloat("_AddSrcBlend", 1f);
                        mat.SetFloat("_AddDstBlend", 1f);
                        mat.SetFloat("_AddSrcBlend2", 0f);
                        mat.SetFloat("_AddDstBlend2", 10f);
                        mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack | MaterialGlobalIlluminationFlags.RealtimeEmissive;
                        mat.renderQueue = 3101;
                        mat.enableInstancing = true;

                    }
                    Texture emissionTexture = mat.GetTexture("_EmissionMap");
                    if (emissionTexture || mat.name.Contains("illum"))
                    {
                        mat.EnableKeyword("MARMO_EMISSION");
                        mat.SetFloat("_EnableGlow", 1f);
                        mat.SetTexture("_Illum", emissionTexture);
                    }
                }
            }
        }
    }

    public static class GameObjectExtensions
    {
        public static GameObject SearchChild(this GameObject gameObject, string byName)
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.name == byName)
                {
                    return child.gameObject;
                }
                GameObject recursive = SearchChild(child.gameObject, byName);
                if (recursive)
                {
                    return recursive;
                }
            }
            return null;
        }
        public static T SearchComponent<T>(this GameObject gameObject, string gameObjectName)
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.name == gameObjectName)
                {
                    return child.gameObject.GetComponent<T>();
                }
                GameObject recursive = SearchChild(child.gameObject, gameObjectName);
                if (recursive)
                {
                    return recursive.GetComponent<T>();
                }
            }
            return default;
        }
    }
}
