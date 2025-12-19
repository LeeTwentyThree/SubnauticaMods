using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using UnityEngine;

namespace KallieʼsPropPack.Prefabs.Grasses;

public class ColoredGrass
{
    private PrefabInfo Info { get; }
    private string CloneClassId { get; }
    
    private Color MainColor { get; set;}
    private Color SpecColor { get; set;}
    private bool HasShadows { get; set; }
    
    public ColoredGrass(string id, string cloneClassId)
    {
        Info = PrefabInfo.WithTechType(id);
        CloneClassId = cloneClassId;
    }

    public ColoredGrass WithColor(Color color)
    {
        MainColor = color;
        SpecColor = color;
        return this;
    }
    
    public ColoredGrass WithColor(Color color, Color specColor)
    {
        MainColor = color;
        SpecColor = specColor;
        return this;
    }

    public ColoredGrass WithShadows()
    {
        HasShadows = true;
        return this;
    }

    public void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(new CloneTemplate(Info, CloneClassId)
        {
            ModifyPrefab = obj =>
            {
                var renderers = obj.GetComponentsInChildren<Renderer>();
                foreach (var renderer in renderers)
                {
                    var material = renderer.material;
                    if (material == null) continue;
                    material.color = MainColor;
                    material.SetColor(ShaderPropertyID._SpecColor, SpecColor);
                    
                    if (!HasShadows)
                    {
                        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    }
                }
            }
        });
        prefab.Register();
    }
}