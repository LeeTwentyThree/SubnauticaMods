﻿using UnityEngine;

namespace EpicImprovedTextures;

public static class TextureUtils
{
    private static readonly int SpecTex = Shader.PropertyToID("_SpecTex");

    public static void ConvertRenderer(Renderer renderer, TextureDatabase database)
    {
        var existingConversion = renderer.gameObject.GetComponent<ConvertedTexture>();
        if (existingConversion != null)
        {
            if (renderer.materials.Length > 0 && renderer.materials[0] != null && renderer.materials[0].mainTexture == existingConversion.expectedTexture)
            {
                return;
            }
        }
        var materials = renderer.materials;
        foreach (var material in materials)
        {
            if (material == null) continue;
            var chosen = database.Textures[Random.Range(0, database.Textures.Count)];
            material.mainTexture = chosen;
            material.SetTexture(SpecTex, chosen);
        }

        var converted = renderer.gameObject.EnsureComponent<ConvertedTexture>();
        if (materials.Length > 0 && materials[0] != null)
        {
            converted.expectedTexture = materials[0].mainTexture;
        }
    } 
}