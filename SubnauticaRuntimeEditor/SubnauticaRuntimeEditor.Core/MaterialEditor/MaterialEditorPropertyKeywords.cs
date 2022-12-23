using System.Collections.Generic;

namespace SubnauticaRuntimeEditor.Core.MaterialEditor
{
	public static class MaterialEditorPropertyKeywords
	{
		public static Dictionary<MaterialEditorProperties, Keywords[]> KEYWORDS = new Dictionary<MaterialEditorProperties, Keywords[]>
		{
			{
				MaterialEditorProperties.Color,
				new Keywords[0]
			},
			{
				MaterialEditorProperties.Color2,
				new Keywords[]
				{
					Keywords.UWE_3COLOR
				}
			},
			{
				MaterialEditorProperties.Color3,
				new Keywords[]
				{
					Keywords.UWE_3COLOR
				}
			},
			{
				MaterialEditorProperties.Mode,
				new Keywords[]
				{
					Keywords.UWE_SIG,
					Keywords.MARMO_SPECMAP
				}
			},
			{
				MaterialEditorProperties.Fresnel,
				new Keywords[]
				{
					Keywords.UWE_SIG,
					Keywords.MARMO_SPECMAP
				}
			},
			{
				MaterialEditorProperties.Shininess,
				new Keywords[]
				{
					Keywords.UWE_SIG,
					Keywords.MARMO_SPECMAP
				}
			},
			{
				MaterialEditorProperties.SpecInt,
				new Keywords[]
				{
					Keywords.UWE_SIG,
					Keywords.MARMO_SPECMAP
				}
			},
			{
				MaterialEditorProperties.EnableGlow,
				new Keywords[]
				{
					Keywords.MARMO_EMISSION
				}
			},
			{
				MaterialEditorProperties.EnableLighting,
				new Keywords[0]
			},
			{
				MaterialEditorProperties.SpecColor,
				new Keywords[0]
			},
			{
				MaterialEditorProperties.SrcBlend,
				new Keywords[0]
			},
			{
				MaterialEditorProperties.DstBlend,
				new Keywords[0]
			},
			{
				MaterialEditorProperties.SrcBlend2,
				new Keywords[0]
			},
			{
				MaterialEditorProperties.DstBlend2,
				new Keywords[0]
			},
			{
				MaterialEditorProperties.AddSrcBlend,
				new Keywords[0]
			},
			{
				MaterialEditorProperties.AddDstBlend,
				new Keywords[0]
			},
			{
				MaterialEditorProperties.AddSrcBlend2,
				new Keywords[0]
			},
			{
				MaterialEditorProperties.AddDstBlend2,
				new Keywords[0]
			},
			{
				MaterialEditorProperties.EnableMisc,
				new Keywords[0]
			},
			{
				MaterialEditorProperties.ZWrite,
				new Keywords[0]
			},
			{
				MaterialEditorProperties.ZOffset,
				new Keywords[0]
			},
			{
				MaterialEditorProperties.EnableCutOff,
				new Keywords[]
				{
					Keywords.MARMO_ALPHA_CLIP
				}
			},
			{
				MaterialEditorProperties.Cutoff,
				new Keywords[]
				{
					Keywords.MARMO_ALPHA_CLIP,
					Keywords.FX_BUILDING
				}
			},
			{
				MaterialEditorProperties.EnableDitherAlpha,
				new Keywords[]
				{
					Keywords.UWE_DITHERALPHA
				}
			},
			{
				MaterialEditorProperties.EnableVrFadeOut,
				new Keywords[]
				{
					Keywords.UWE_VR_FADEOUT
				}
			},
			{
				MaterialEditorProperties.IBLreductionAtNight,
				new Keywords[0]
			},
			{
				MaterialEditorProperties.EnableSimpleGlass,
				new Keywords[]
				{
					Keywords.MARMO_SIMPLE_GLASS
				}
			},
			{
				MaterialEditorProperties.EnableVertexColor,
				new Keywords[]
				{
					Keywords.MARMO_VERTEX_COLOR
				}
			},
			{
				MaterialEditorProperties.EnableSchoolFish,
				new Keywords[]
				{
					Keywords.UWE_SCHOOLFISH
				}
			},
			{
				MaterialEditorProperties.EnableMainMaps,
				new Keywords[0]
			},
			{
				MaterialEditorProperties.GlowColor,
				new Keywords[0]
			},
			{
				MaterialEditorProperties.GlowUVfromVC,
				new Keywords[]
				{
					Keywords.GLOW_UV_FROM_VERTECCOLOR,
					Keywords.MARMO_EMISSION
				}
			},
			{
				MaterialEditorProperties.GlowStrength,
				new Keywords[]
				{
					Keywords.MARMO_EMISSION,
					Keywords.FX_IONCRYSTAL
				}
			},
			{
				MaterialEditorProperties.GlowStrengthNight,
				new Keywords[]
				{
					Keywords.MARMO_EMISSION,
					Keywords.FX_IONCRYSTAL
				}
			},
			{
				MaterialEditorProperties.EmissionLM,
				new Keywords[]
				{
					Keywords.MARMO_EMISSION,
					Keywords.FX_IONCRYSTAL
				}
			},
			{
				MaterialEditorProperties.EmissionLMNight,
				new Keywords[]
				{
					Keywords.MARMO_EMISSION,
					Keywords.FX_IONCRYSTAL
				}
			},
			{
				MaterialEditorProperties.EnableDetailMaps,
				new Keywords[]
				{
					Keywords.UWE_DETAILMAP
				}
			},
			{
				MaterialEditorProperties.DetailIntensities,
				new Keywords[]
				{
					Keywords.UWE_DETAILMAP
				}
			},
			{
				MaterialEditorProperties.EnableLightmap,
				new Keywords[]
				{
					Keywords.UWE_LIGHTMAP
				}
			},
			{
				MaterialEditorProperties.LightmapStrength,
				new Keywords[]
				{
					Keywords.UWE_LIGHTMAP
				}
			},
			{
				MaterialEditorProperties.Enable3Color,
				new Keywords[]
				{
					Keywords.UWE_3COLOR
				}
			},
			{
				MaterialEditorProperties.SpecColor2,
				new Keywords[]
				{
					Keywords.UWE_3COLOR
				}
			},
			{
				MaterialEditorProperties.SpecColor3,
				new Keywords[]
				{
					Keywords.UWE_3COLOR
				}
			},
			{
				MaterialEditorProperties.DeformParams,
				new Keywords[]
				{
					Keywords.FX_DEFORM,
					Keywords.FX_MESMER,
					Keywords.FX_BLEEDER
				}
			},
			{
				MaterialEditorProperties.FillSack,
				new Keywords[]
				{
					Keywords.FX_BLEEDER
				}
			},
			{
				MaterialEditorProperties.OverlayStrength,
				new Keywords[]
				{
					Keywords.FX_PROPULSIONCANNON
				}
			},
			{
				MaterialEditorProperties.GlowScrollColor,
				new Keywords[]
				{
					Keywords.FX_PROPULSIONCANNON
				}
			},
			{
				MaterialEditorProperties.Hypnotize,
				new Keywords[]
				{
					Keywords.FX_MESMER
				}
			},
			{
				MaterialEditorProperties.ScrollColor,
				new Keywords[]
				{
					Keywords.FX_MESMER,
					Keywords.FX_BLEEDER
				}
			},
			{
				MaterialEditorProperties.ColorStrength,
				new Keywords[]
				{
					Keywords.FX_MESMER,
					Keywords.FX_BLEEDER
				}
			},
			{
				MaterialEditorProperties.GlowMaskSpeed,
				new Keywords[1]
			},
			{
				MaterialEditorProperties.ScrollSpeed,
				new Keywords[]
				{
					Keywords.FX_MESMER,
					Keywords.FX_BLEEDER,
					Keywords.FX_PROPULSIONCANNON
				}
			},
			{
				MaterialEditorProperties.DetailsColor,
				new Keywords[]
				{
					Keywords.FX_IONCRYSTAL
				}
			},
			{
				MaterialEditorProperties.SquaresColor,
				new Keywords[]
				{
					Keywords.FX_IONCRYSTAL
				}
			},
			{
				MaterialEditorProperties.SquaresTile,
				new Keywords[]
				{
					Keywords.FX_IONCRYSTAL
				}
			},
			{
				MaterialEditorProperties.SquaresSpeed,
				new Keywords[]
				{
					Keywords.FX_IONCRYSTAL
				}
			},
			{
				MaterialEditorProperties.SquaresIntensityPow,
				new Keywords[]
				{
					Keywords.FX_IONCRYSTAL
				}
			},
			{
				MaterialEditorProperties.NoiseSpeed,
				new Keywords[]
				{
					Keywords.FX_IONCRYSTAL
				}
			},
			{
				MaterialEditorProperties.FakeSSSparams,
				new Keywords[]
				{
					Keywords.FX_IONCRYSTAL
				}
			},
			{
				MaterialEditorProperties.FakeSSSSpeed,
				new Keywords[]
				{
					Keywords.FX_IONCRYSTAL
				}
			},
			{
				MaterialEditorProperties.BorderColor,
				new Keywords[]
				{
					Keywords.FX_BUILDING
				}
			},
			{
				MaterialEditorProperties.Built,
				new Keywords[]
				{
					Keywords.FX_BUILDING
				}
			},
			{
				MaterialEditorProperties.BuildParams,
				new Keywords[]
				{
					Keywords.FX_BUILDING
				}
			},
			{
				MaterialEditorProperties.BuildLinear,
				new Keywords[]
				{
					Keywords.FX_BUILDING
				}
			},
			{
				MaterialEditorProperties.NoiseThickness,
				new Keywords[]
				{
					Keywords.FX_BUILDING
				}
			},
			{
				MaterialEditorProperties.NoiseStr,
				new Keywords[]
				{
					Keywords.FX_BUILDING
				}
			},
			{
				MaterialEditorProperties.Scale,
				new Keywords[]
				{
					Keywords.UWE_WAVING,
					Keywords.FX_ROPE,
					Keywords.FX_SINWAVE,
					Keywords.FX_KELP
				}
			},
			{
				MaterialEditorProperties.Frequency,
				new Keywords[]
				{
					Keywords.UWE_WAVING,
					Keywords.FX_ROPE,
					Keywords.FX_SINWAVE,
					Keywords.FX_KELP
				}
			},
			{
				MaterialEditorProperties.Speed,
				new Keywords[]
				{
					Keywords.UWE_WAVING,
					Keywords.FX_ROPE,
					Keywords.FX_SINWAVE,
					Keywords.FX_KELP
				}
			},
			{
				MaterialEditorProperties.ObjectUp,
				new Keywords[]
				{
					Keywords.UWE_WAVING
				}
			},
			{
				MaterialEditorProperties.WaveUpMin,
				new Keywords[]
				{
					Keywords.UWE_WAVING
				}
			},
			{
				MaterialEditorProperties.Fallof,
				new Keywords[]
				{
					Keywords.UWE_WAVING,
					Keywords.FX_ROPE
				}
			},
			{
				MaterialEditorProperties.RopeGravity,
				new Keywords[]
				{
					Keywords.FX_ROPE
				}
			},
			{
				MaterialEditorProperties.minYpos,
				new Keywords[]
				{
					Keywords.FX_KELP,
					Keywords.FX_BUILDING
				}
			},
			{
				MaterialEditorProperties.maxYpos,
				new Keywords[]
				{
					Keywords.FX_KELP,
					Keywords.FX_BUILDING
				}
			},
			{
				MaterialEditorProperties.EnableBurst,
				new Keywords[]
				{
					Keywords.FX_BURST
				}
			},
			{
				MaterialEditorProperties.Displacement,
				new Keywords[]
				{
					Keywords.FX_BURST
				}
			},
			{
				MaterialEditorProperties.BurstStrength,
				new Keywords[]
				{
					Keywords.FX_BURST
				}
			},
			{
				MaterialEditorProperties.Range,
				new Keywords[]
				{
					Keywords.FX_BURST
				}
			},
			{
				MaterialEditorProperties.ClipRange,
				new Keywords[]
				{
					Keywords.FX_BURST
				}
			},
			{
				MaterialEditorProperties.EnableInfection,
				new Keywords[]
				{
					Keywords.UWE_INFECTION
				}
			},
			{
				MaterialEditorProperties.EnablePlayerInfection,
				new Keywords[]
				{
					Keywords.UWE_PLAYERINFECTION
				}
			},
			{
				MaterialEditorProperties.InfectionHeightStrength,
				new Keywords[]
				{
					Keywords.UWE_INFECTION,
					Keywords.UWE_PLAYERINFECTION
				}
			},
			{
				MaterialEditorProperties.InfectionScale,
				new Keywords[]
				{
					Keywords.UWE_INFECTION,
					Keywords.UWE_PLAYERINFECTION
				}
			},
			{
				MaterialEditorProperties.InfectionOffset,
				new Keywords[]
				{
					Keywords.UWE_INFECTION,
					Keywords.UWE_PLAYERINFECTION
				}
			},
			{
				MaterialEditorProperties.InfectionSpeed,
				new Keywords[]
				{
					Keywords.UWE_PLAYERINFECTION
				}
			},
			{
				MaterialEditorProperties.MyCullVariable,
				new Keywords[0]
			}
		};
	}
}
