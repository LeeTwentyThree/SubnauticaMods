using System;
using System.Collections.Generic;

namespace SubnauticaRuntimeEditor.Core.MaterialEditor
{
	public static class MaterialEditorPropertyTypes
	{
		public static Dictionary<MaterialEditorProperties, PropertyType> TYPES = new Dictionary<MaterialEditorProperties, PropertyType>
		{
			{
				MaterialEditorProperties.Color,
				new PropertyTypeColor("_Color")
			},
			{
				MaterialEditorProperties.Mode,
				new PropertyTypeToggle("_Mode", 0f, 100f)
			},
			{
				MaterialEditorProperties.SrcBlend,
				new PropertyTypeToggle("_SrcBlend", 0f, 100f)
			},
			{
				MaterialEditorProperties.DstBlend,
				new PropertyTypeToggle("_DstBlend", 0f, 100f)
			},
			{
				MaterialEditorProperties.SrcBlend2,
				new PropertyTypeToggle("_SrcBlend2", 0f, 100f)
			},
			{
				MaterialEditorProperties.DstBlend2,
				new PropertyTypeToggle("_DstBlend2", 0f, 100f)
			},
			{
				MaterialEditorProperties.AddSrcBlend,
				new PropertyTypeToggle("_AddSrcBlend", 0f, 100f)
			},
			{
				MaterialEditorProperties.AddDstBlend,
				new PropertyTypeToggle("_AddDstBlend", 0f, 100f)
			},
			{
				MaterialEditorProperties.AddSrcBlend2,
				new PropertyTypeToggle("_AddSrcBlend2", 0f, 100f)
			},
			{
				MaterialEditorProperties.AddDstBlend2,
				new PropertyTypeToggle("_AddDstBlend2", 0f, 100f)
			},
			{
				MaterialEditorProperties.EnableMisc,
				new PropertyTypeToggle("_EnableMisc", 0f, 100f)
			},
			{
				MaterialEditorProperties.MyCullVariable,
				new PropertyTypeToggle("_MyCullVariable", 0f, 100f)
			},
			{
				MaterialEditorProperties.ZWrite,
				new PropertyTypeFloat("_ZWrite")
			},
			{
				MaterialEditorProperties.ZOffset,
				new PropertyTypeToggle("_ZOffset", 0f, 100f)
			},
			{
				MaterialEditorProperties.EnableCutOff,
				new PropertyTypeToggle("_EnableCutOff", 0f, 100f)
			},
			{
				MaterialEditorProperties.Cutoff,
				new PropertyTypeToggle("_Cutoff", 0f, 1f)
			},
			{
				MaterialEditorProperties.EnableDitherAlpha,
				new PropertyTypeToggle("_EnableDitherAlpha", 0f, 100f)
			},
			{
				MaterialEditorProperties.EnableVrFadeOut,
				new PropertyTypeToggle("_EnableVrFadeOut", 0f, 100f)
			},
			{
				MaterialEditorProperties.EnableLighting,
				new PropertyTypeToggle("_EnableLighting", 0f, 100f)
			},
			{
				MaterialEditorProperties.IBLreductionAtNight,
				new PropertyTypeToggle("_IBLreductionAtNight", 0f, 1f)
			},
			{
				MaterialEditorProperties.EnableSimpleGlass,
				new PropertyTypeToggle("_EnableSimpleGlass", 0f, 100f)
			},
			{
				MaterialEditorProperties.EnableVertexColor,
				new PropertyTypeToggle("_EnableVertexColor", 0f, 100f)
			},
			{
				MaterialEditorProperties.EnableSchoolFish,
				new PropertyTypeToggle("_EnableSchoolFish", 0f, 100f)
			},
			{
				MaterialEditorProperties.EnableMainMaps,
				new PropertyTypeToggle("_EnableMainMaps", 0f, 100f)
			},
			{
				MaterialEditorProperties.MarmoSpecEnum,
				new PropertyTypeToggle("_MarmoSpecEnum", 0f, 100f)
			},
			{
				MaterialEditorProperties.SpecColor,
				new PropertyTypeColor("_SpecColor")
			},
			{
				MaterialEditorProperties.SpecInt,
				new PropertyTypeToggle("_SpecInt", 0f, 100f)
			},
			{
				MaterialEditorProperties.Shininess,
				new PropertyTypeToggle("_Shininess", 2f, 8f)
			},
			{
				MaterialEditorProperties.Fresnel,
				new PropertyTypeToggle("_Fresnel", 0f, 1f)
			},
			{
				MaterialEditorProperties.EnableGlow,
				new PropertyTypeToggle("_EnableGlow", 0f, 100f)
			},
			{
				MaterialEditorProperties.GlowColor,
				new PropertyTypeColor("_GlowColor")
			},
			{
				MaterialEditorProperties.GlowUVfromVC,
				new PropertyTypeToggle("_GlowUVfromVC", 0f, 100f)
			},
			{
				MaterialEditorProperties.GlowStrength,
				new PropertyTypeToggle("_GlowStrength", 0f, 100f)
			},
			{
				MaterialEditorProperties.GlowStrengthNight,
				new PropertyTypeToggle("_GlowStrengthNight", 0f, 100f)
			},
			{
				MaterialEditorProperties.EmissionLM,
				new PropertyTypeToggle("_EmissionLM", 0f, 100f)
			},
			{
				MaterialEditorProperties.EmissionLMNight,
				new PropertyTypeToggle("_EmissionLMNight", 0f, 100f)
			},
			{
				MaterialEditorProperties.EnableDetailMaps,
				new PropertyTypeToggle("_EnableDetailMaps", 0f, 100f)
			},
			{
				MaterialEditorProperties.DetailIntensities,
				new PropertyTypeVector4("_DetailIntensities")
			},
			{
				MaterialEditorProperties.EnableLightmap,
				new PropertyTypeToggle("_EnableLightmap", 0f, 100f)
			},
			{
				MaterialEditorProperties.LightmapStrength,
				new PropertyTypeToggle("_LightmapStrength", 0f, 100f)
			},
			{
				MaterialEditorProperties.Enable3Color,
				new PropertyTypeToggle("_Enable3Color", 0f, 100f)
			},
			{
				MaterialEditorProperties.Color2,
				new PropertyTypeColor("_Color2")
			},
			{
				MaterialEditorProperties.Color3,
				new PropertyTypeColor("_Color3")
			},
			{
				MaterialEditorProperties.SpecColor2,
				new PropertyTypeColor("_SpecColor2")
			},
			{
				MaterialEditorProperties.SpecColor3,
				new PropertyTypeColor("_SpecColor3")
			},
			{
				MaterialEditorProperties.FX,
				new PropertyTypeToggle("FX", 0f, 100f)
			},
			{
				MaterialEditorProperties.DeformParams,
				new PropertyTypeVector4("_DeformParams")
			},
			{
				MaterialEditorProperties.FillSack,
				new PropertyTypeToggle("_FillSack", 0f, 1f)
			},
			{
				MaterialEditorProperties.OverlayStrength,
				new PropertyTypeToggle("_OverlayStrength", 0f, 1f)
			},
			{
				MaterialEditorProperties.GlowScrollColor,
				new PropertyTypeVector4("_GlowScrollColor")
			},
			{
				MaterialEditorProperties.Hypnotize,
				new PropertyTypeToggle("_Hypnotize", 0f, 1f)
			},
			{
				MaterialEditorProperties.ScrollColor,
				new PropertyTypeColor("_ScrollColor")
			},
			{
				MaterialEditorProperties.ColorStrength,
				new PropertyTypeVector4("_ColorStrength")
			},
			{
				MaterialEditorProperties.GlowMaskSpeed,
				new PropertyTypeVector4("_GlowMaskSpeed")
			},
			{
				MaterialEditorProperties.ScrollSpeed,
				new PropertyTypeVector4("_ScrollSpeed")
			},
			{
				MaterialEditorProperties.DetailsColor,
				new PropertyTypeColor("_DetailsColor")
			},
			{
				MaterialEditorProperties.SquaresColor,
				new PropertyTypeColor("_SquaresColor")
			},
			{
				MaterialEditorProperties.SquaresTile,
				new PropertyTypeToggle("_SquaresTile", 0f, 100f)
			},
			{
				MaterialEditorProperties.SquaresSpeed,
				new PropertyTypeToggle("_SquaresSpeed", 0f, 100f)
			},
			{
				MaterialEditorProperties.SquaresIntensityPow,
				new PropertyTypeToggle("_SquaresIntensityPow", 0f, 100f)
			},
			{
				MaterialEditorProperties.NoiseSpeed,
				new PropertyTypeVector4("_NoiseSpeed")
			},
			{
				MaterialEditorProperties.FakeSSSparams,
				new PropertyTypeVector4("_FakeSSSparams")
			},
			{
				MaterialEditorProperties.FakeSSSSpeed,
				new PropertyTypeVector4("_FakeSSSSpeed")
			},
			{
				MaterialEditorProperties.BorderColor,
				new PropertyTypeColor("_BorderColor")
			},
			{
				MaterialEditorProperties.Built,
				new PropertyTypeToggle("_Built", 0f, 100f)
			},
			{
				MaterialEditorProperties.BuildParams,
				new PropertyTypeVector4("_BuildParams")
			},
			{
				MaterialEditorProperties.BuildLinear,
				new PropertyTypeToggle("_BuildLinear", 0f, 100f)
			},
			{
				MaterialEditorProperties.NoiseThickness,
				new PropertyTypeToggle("_NoiseThickness", 0f, 100f)
			},
			{
				MaterialEditorProperties.NoiseStr,
				new PropertyTypeToggle("_NoiseStr", 0f, 100f)
			},
			{
				MaterialEditorProperties.FX_Vertex,
				new PropertyTypeToggle("FX_Vertex", 0f, 100f)
			},
			{
				MaterialEditorProperties.Scale,
				new PropertyTypeVector4("_Scale")
			},
			{
				MaterialEditorProperties.Frequency,
				new PropertyTypeVector4("_Frequency")
			},
			{
				MaterialEditorProperties.Speed,
				new PropertyTypeVector4("_Speed")
			},
			{
				MaterialEditorProperties.ObjectUp,
				new PropertyTypeVector4("_ObjectUp")
			},
			{
				MaterialEditorProperties.WaveUpMin,
				new PropertyTypeToggle("_WaveUpMin", 0f, 100f)
			},
			{
				MaterialEditorProperties.Fallof,
				new PropertyTypeToggle("_Fallof", 0f, 10f)
			},
			{
				MaterialEditorProperties.RopeGravity,
				new PropertyTypeToggle("_RopeGravity", 0f, 100f)
			},
			{
				MaterialEditorProperties.minYpos,
				new PropertyTypeToggle("_minYpos", 0f, 100f)
			},
			{
				MaterialEditorProperties.maxYpos,
				new PropertyTypeToggle("_maxYpos", 0f, 100f)
			},
			{
				MaterialEditorProperties.EnableBurst,
				new PropertyTypeToggle("_EnableBurst", 0f, 100f)
			},
			{
				MaterialEditorProperties.Displacement,
				new PropertyTypeToggle("_Displacement", 0f, 5f)
			},
			{
				MaterialEditorProperties.BurstStrength,
				new PropertyTypeToggle("_BurstStrength", 0f, 1f)
			},
			{
				MaterialEditorProperties.Range,
				new PropertyTypeVector4("_Range")
			},
			{
				MaterialEditorProperties.ClipRange,
				new PropertyTypeToggle("_ClipRange", 0f, 100f)
			},
			{
				MaterialEditorProperties.EnableInfection,
				new PropertyTypeToggle("_EnableInfection", 0f, 100f)
			},
			{
				MaterialEditorProperties.EnablePlayerInfection,
				new PropertyTypeToggle("_EnablePlayerInfection", 0f, 100f)
			},
			{
				MaterialEditorProperties.InfectionHeightStrength,
				new PropertyTypeToggle("_InfectionHeightStrength", 0f, 100f)
			},
			{
				MaterialEditorProperties.InfectionScale,
				new PropertyTypeVector4("_InfectionScale")
			},
			{
				MaterialEditorProperties.InfectionOffset,
				new PropertyTypeVector4("_InfectionOffset")
			},
			{
				MaterialEditorProperties.InfectionSpeed,
				new PropertyTypeVector4("_InfectionSpeed")
			},
			{
				MaterialEditorProperties.FlowSpeed,
				new PropertyTypeToggle("_FlowSpeed", 0f, 100f)
			},
			{
				MaterialEditorProperties.EmissiveCut,
				new PropertyTypeToggle("_EmissiveCut", 0.01f, 0.99f)
			},
			{
				MaterialEditorProperties.GridThickness,
				new PropertyTypeToggle("_GridThickness", 0f, 100f)
			},
			{
				MaterialEditorProperties.GridSpacing,
				new PropertyTypeToggle("_GridSpacing", 0f, 100f)
			},
			{
				MaterialEditorProperties.Tint,
				new PropertyTypeColor("_Tint")
			},
			{
				MaterialEditorProperties.FresnelPow,
				new PropertyTypeToggle("_FresnelPow", 0f, 100f)
			},
			{
				MaterialEditorProperties.FresnelFade,
				new PropertyTypeToggle("_FresnelFade", 0f, 100f)
			},
			{
				MaterialEditorProperties.Emission,
				new PropertyTypeToggle("_Emission", 0f, 1f)
			},
			{
				MaterialEditorProperties.Amount,
				new PropertyTypeToggle("_Amount", 0f, 1f)
			},
			{
				MaterialEditorProperties.TopBorder,
				new PropertyTypeToggle("_TopBorder", 0f, 100f)
			},
			{
				MaterialEditorProperties.BottomBorder,
				new PropertyTypeToggle("_BottomBorder", 0f, 100f)
			},
			{
				MaterialEditorProperties.ZWriteMode,
				new PropertyTypeToggle("_ZWriteMode", 0f, 100f)
			},
			{
				MaterialEditorProperties.ColorStrengthAtNight,
				new PropertyTypeVector4("_ColorStrengthAtNight")
			},
			{
				MaterialEditorProperties.EmissUV,
				new PropertyTypeVector4("_EmissUV")
			},
			{
				MaterialEditorProperties.Emiss2UV,
				new PropertyTypeVector4("_Emiss2UV")
			},
			{
				MaterialEditorProperties.ScrollSpeed2,
				new PropertyTypeVector4("_ScrollSpeed2")
			},
			{
				MaterialEditorProperties.FadeAmount,
				new PropertyTypeToggle("_FadeAmount", 0f, 100f)
			},
			{
				MaterialEditorProperties.EmissiveStrengh,
				new PropertyTypeToggle("_EmissiveStrengh", 0f, 100f)
			},
			{
				MaterialEditorProperties.scrollSpeed,
				new PropertyTypeVector4("_scrollSpeed")
			},
			{
				MaterialEditorProperties.distortScrollSpeed,
				new PropertyTypeVector4("_distortScrollSpeed")
			},
			{
				MaterialEditorProperties.ClipOffset,
				new PropertyTypeToggle("_ClipOffset", 0f, 100f)
			},
			{
				MaterialEditorProperties.ClipFade,
				new PropertyTypeToggle("_ClipFade", 0f, 100f)
			},
			{
				MaterialEditorProperties.RimPower,
				new PropertyTypeToggle("_RimPower", 0f, 100f)
			},
			{
				MaterialEditorProperties.InvFade,
				new PropertyTypeToggle("_InvFade", 0.01f, 5f)
			},
			{
				MaterialEditorProperties.IslandClouds,
				new PropertyTypeToggle("_IslandClouds", 0f, 100f)
			},
			{
				MaterialEditorProperties.DomeClouds,
				new PropertyTypeToggle("_DomeClouds", 0f, 100f)
			},
			{
				MaterialEditorProperties.PlanetClouds,
				new PropertyTypeToggle("_PlanetClouds", 0f, 100f)
			},
			{
				MaterialEditorProperties.LightAmount,
				new PropertyTypeToggle("_LightAmount", 0f, 100f)
			},
			{
				MaterialEditorProperties.StepSize,
				new PropertyTypeToggle("_StepSize", 0f, 100f)
			},
			{
				MaterialEditorProperties.CloudsScatteringExponent,
				new PropertyTypeToggle("_CloudsScatteringExponent", 0f, 100f)
			},
			{
				MaterialEditorProperties.CloudsScatteringMultiplier,
				new PropertyTypeToggle("_CloudsScatteringMultiplier", 0f, 100f)
			},
			{
				MaterialEditorProperties.CloudsAttenuation,
				new PropertyTypeToggle("_CloudsAttenuation", 0f, 100f)
			},
			{
				MaterialEditorProperties.SkyColorMultiplier,
				new PropertyTypeToggle("_SkyColorMultiplier", 0f, 100f)
			},
			{
				MaterialEditorProperties.SunColorMultiplier,
				new PropertyTypeToggle("_SunColorMultiplier", 0f, 100f)
			},
			{
				MaterialEditorProperties.ChannelLerp,
				new PropertyTypeVector4("_ChannelLerp")
			},
			{
				MaterialEditorProperties.ChannelLerp2,
				new PropertyTypeVector4("_ChannelLerp2")
			},
			{
				MaterialEditorProperties.LightMin,
				new PropertyTypeToggle("_LightMin", 0f, 100f)
			},
			{
				MaterialEditorProperties.LightMul,
				new PropertyTypeToggle("_LightMul", 0f, 100f)
			},
			{
				MaterialEditorProperties.HorizonFallof,
				new PropertyTypeToggle("_HorizonFallof", 0f, 100f)
			},
			{
				MaterialEditorProperties.FadeInSkyColor,
				new PropertyTypeToggle("_FadeInSkyColor", 0f, 100f)
			},
			{
				MaterialEditorProperties.AlphaPow,
				new PropertyTypeToggle("_AlphaPow", 0f, 100f)
			},
			{
				MaterialEditorProperties.DeformSpeed,
				new PropertyTypeVector4("_DeformSpeed")
			},
			{
				MaterialEditorProperties.MixStrength,
				new PropertyTypeVector4("_MixStrength")
			},
			{
				MaterialEditorProperties.MixSpeed,
				new PropertyTypeVector4("_MixSpeed")
			},
			{
				MaterialEditorProperties.SurfFade,
				new PropertyTypeToggle("_SurfFade", 0f, 100f)
			},
			{
				MaterialEditorProperties.SeaLevel,
				new PropertyTypeToggle("_SeaLevel", 0f, 100f)
			},
			{
				MaterialEditorProperties.TintColor,
				new PropertyTypeColor("_TintColor")
			},
			{
				MaterialEditorProperties.LiquidColor,
				new PropertyTypeColor("_LiquidColor")
			},
			{
				MaterialEditorProperties.BubblesColor,
				new PropertyTypeColor("_BubblesColor")
			},
			{
				MaterialEditorProperties.LiquidGlow,
				new PropertyTypeToggle("_LiquidGlow", 0f, 100f)
			},
			{
				MaterialEditorProperties.BubblesGlow,
				new PropertyTypeToggle("_BubblesGlow", 0f, 100f)
			},
			{
				MaterialEditorProperties.levelOffset,
				new PropertyTypeToggle("_levelOffset", 0f, 100f)
			},
			{
				MaterialEditorProperties.BlurStrength,
				new PropertyTypeToggle("_BlurStrength", 0f, 100f)
			},
			{
				MaterialEditorProperties.BlurWidth,
				new PropertyTypeToggle("_BlurWidth", 0f, 100f)
			},
			{
				MaterialEditorProperties.rainAmount,
				new PropertyTypeToggle("_rainAmount", 0f, 100f)
			},
			{
				MaterialEditorProperties.SurfLevel,
				new PropertyTypeToggle("_SurfLevel", 0f, 100f)
			},
			{
				MaterialEditorProperties.snowAmount,
				new PropertyTypeToggle("_snowAmount", 0f, 100f)
			},
			{
				MaterialEditorProperties.AffectedByDayNightCycle,
				new PropertyTypeToggle("_AffectedByDayNightCycle", 0f, 100f)
			},
			{
				MaterialEditorProperties.FX_LightMode,
				new PropertyTypeToggle("FX_LightMode", 0f, 100f)
			},
			{
				MaterialEditorProperties.SelfIllumination,
				new PropertyTypeToggle("_SelfIllumination", 0f, 1f)
			},
			{
				MaterialEditorProperties.LightWrapAround,
				new PropertyTypeToggle("_LightWrapAround", 0f, 1f)
			},
			{
				MaterialEditorProperties.NormalSharpness,
				new PropertyTypeToggle("_NormalSharpness", 0f, 1f)
			},
			{
				MaterialEditorProperties.LightDesaturation,
				new PropertyTypeToggle("_LightDesaturation", 0f, 1f)
			},
			{
				MaterialEditorProperties.MainTex_Speed,
				new PropertyTypeVector4("_MainTex_Speed")
			},
			{
				MaterialEditorProperties.MainTex2_Speed,
				new PropertyTypeVector4("_MainTex2_Speed")
			},
			{
				MaterialEditorProperties.DeformMap_Speed,
				new PropertyTypeVector4("_DeformMap_Speed")
			},
			{
				MaterialEditorProperties.RefractMap_Speed,
				new PropertyTypeVector4("_RefractMap_Speed")
			},
			{
				MaterialEditorProperties.LocalFloodLevel,
				new PropertyTypeToggle("_LocalFloodLevel", 0f, 100f)
			},
			{
				MaterialEditorProperties.gravityStrength,
				new PropertyTypeToggle("_gravityStrength", 0f, 100f)
			},
			{
				MaterialEditorProperties.BoxMin,
				new PropertyTypeVector4("_BoxMin")
			},
			{
				MaterialEditorProperties.BoxMax,
				new PropertyTypeVector4("_BoxMax")
			},
			{
				MaterialEditorProperties.Visibility,
				new PropertyTypeToggle("_Visibility", 0f, 100f)
			},
			{
				MaterialEditorProperties.BIsForwardLighting,
				new PropertyTypeToggle("_BIsForwardLighting", 0f, 100f)
			},
			{
				MaterialEditorProperties.AffectedBySkyExposure,
				new PropertyTypeToggle("_AffectedBySkyExposure", 0f, 100f)
			},
			{
				MaterialEditorProperties.CylRadius,
				new PropertyTypeToggle("_CylRadius", 0f, 100f)
			},
			{
				MaterialEditorProperties.MinY,
				new PropertyTypeToggle("_MinY", 0f, 100f)
			},
			{
				MaterialEditorProperties.MaxY,
				new PropertyTypeToggle("_MaxY", 0f, 100f)
			},
			{
				MaterialEditorProperties.SolidColor,
				new PropertyTypeColor("_SolidColor")
			},
			{
				MaterialEditorProperties.Intensity,
				new PropertyTypeToggle("_Intensity", 0f, 1f)
			},
			{
				MaterialEditorProperties.ImpactIntensity,
				new PropertyTypeToggle("_ImpactIntensity", 0f, 1f)
			},
			{
				MaterialEditorProperties.ImpactPosition,
				new PropertyTypeVector4("_ImpactPosition")
			},
			{
				MaterialEditorProperties.ImpactParams,
				new PropertyTypeVector4("_ImpactParams")
			},
			{
				MaterialEditorProperties.EnabledSize,
				new PropertyTypeToggle("_EnabledSize", 0f, 100f)
			},
			{
				MaterialEditorProperties.DisabledSize,
				new PropertyTypeToggle("_DisabledSize", 0f, 100f)
			},
			{
				MaterialEditorProperties.WobbleParams,
				new PropertyTypeVector4("_WobbleParams")
			},
			{
				MaterialEditorProperties.PingColor,
				new PropertyTypeColor("_PingColor")
			},
			{
				MaterialEditorProperties.PingCenter,
				new PropertyTypeVector4("_PingCenter")
			},
			{
				MaterialEditorProperties.PingFrequency,
				new PropertyTypeToggle("_PingFrequency", 0f, 100f)
			},
			{
				MaterialEditorProperties.DitherIntensity,
				new PropertyTypeToggle("_DitherIntensity", 0f, 1f)
			},
			{
				MaterialEditorProperties.FragColor,
				new PropertyTypeColor("_FragColor")
			},
			{
				MaterialEditorProperties.RefractStrength,
				new PropertyTypeVector4("_RefractStrength")
			},
			{
				MaterialEditorProperties.RefractUV,
				new PropertyTypeVector4("_RefractUV")
			},
			{
				MaterialEditorProperties.RefractSpeed,
				new PropertyTypeVector4("_RefractSpeed")
			},
			{
				MaterialEditorProperties.DeformUV,
				new PropertyTypeVector4("_DeformUV")
			},
			{
				MaterialEditorProperties.FragsUV,
				new PropertyTypeVector4("_FragsUV")
			},
			{
				MaterialEditorProperties.ScrollSpeed3,
				new PropertyTypeVector4("_ScrollSpeed3")
			},
			{
				MaterialEditorProperties.DeformStrengthFrag,
				new PropertyTypeToggle("_DeformStrengthFrag", 0f, 100f)
			},
			{
				MaterialEditorProperties.MinShadingSmoothness,
				new PropertyTypeToggle("_MinShadingSmoothness", 0f, 100f)
			},
			{
				MaterialEditorProperties.MaxShadingSmoothness,
				new PropertyTypeToggle("_MaxShadingSmoothness", 0f, 100f)
			},
			{
				MaterialEditorProperties.ScanIntensity,
				new PropertyTypeToggle("_ScanIntensity", 0f, 100f)
			},
			{
				MaterialEditorProperties.WireframeDensity,
				new PropertyTypeToggle("_WireframeDensity", 0f, 100f)
			},
			{
				MaterialEditorProperties.WireframeWidth,
				new PropertyTypeToggle("_WireframeWidth", 0f, 100f)
			},
			{
				MaterialEditorProperties.FadeRadius,
				new PropertyTypeToggle("_FadeRadius", 0f, 100f)
			},
			{
				MaterialEditorProperties.NoiseIntensity,
				new PropertyTypeToggle("_NoiseIntensity", 0f, 1f)
			},
			{
				MaterialEditorProperties.RadialFade,
				new PropertyTypeToggle("_RadialFade", 0f, 100f)
			},
			{
				MaterialEditorProperties.ColorCenter,
				new PropertyTypeColor("_ColorCenter")
			},
			{
				MaterialEditorProperties.ColorOuter,
				new PropertyTypeColor("_ColorOuter")
			},
			{
				MaterialEditorProperties.DetailsColorStrength,
				new PropertyTypeVector4("_DetailsColorStrength")
			},
			{
				MaterialEditorProperties.RipplesFrequency,
				new PropertyTypeToggle("_RipplesFrequency", 0f, 100f)
			},
			{
				MaterialEditorProperties.RipplesPow,
				new PropertyTypeToggle("_RipplesPow", 0f, 100f)
			},
			{
				MaterialEditorProperties.RotationSpeed,
				new PropertyTypeToggle("_RotationSpeed", 0f, 100f)
			},
			{
				MaterialEditorProperties.RadiusSqr,
				new PropertyTypeToggle("_RadiusSqr", 0f, 100f)
			},
			{
				MaterialEditorProperties.NoiseScale,
				new PropertyTypeVector4("_NoiseScale")
			},
			{
				MaterialEditorProperties.NoiseOffset,
				new PropertyTypeVector4("_NoiseOffset")
			},
			{
				MaterialEditorProperties.Octaves,
				new PropertyTypeToggle("_Octaves", 0f, 100f)
			},
			{
				MaterialEditorProperties.ClipedValue,
				new PropertyTypeToggle("_ClipedValue", 0f, 100f)
			},
			{
				MaterialEditorProperties.MaskPow,
				new PropertyTypeToggle("_MaskPow", 0f, 100f)
			},
			{
				MaterialEditorProperties.ClipMultiplier,
				new PropertyTypeToggle("_ClipMultiplier", 0f, 100f)
			},
			{
				MaterialEditorProperties.TexScale,
				new PropertyTypeVector4("_TexScale")
			},
			{
				MaterialEditorProperties.Dir,
				new PropertyTypeToggle("_Dir", 0f, 1f)
			},
			{
				MaterialEditorProperties.Cycle,
				new PropertyTypeToggle("_Cycle", 0f, 100f)
			},
			{
				MaterialEditorProperties.MainFoam,
				new PropertyTypeToggle("_MainFoam", 0f, 100f)
			},
			{
				MaterialEditorProperties.TopFoamHeight,
				new PropertyTypeToggle("_TopFoamHeight", 0f, 100f)
			},
			{
				MaterialEditorProperties.BorderFoam,
				new PropertyTypeToggle("_BorderFoam", 0f, 100f)
			},
			{
				MaterialEditorProperties.TopFoamHeightOffset,
				new PropertyTypeToggle("_TopFoamHeightOffset", 0f, 100f)
			},
			{
				MaterialEditorProperties.FoamStrength,
				new PropertyTypeToggle("_FoamStrength", 0f, 100f)
			},
			{
				MaterialEditorProperties.BorderFoamStrength,
				new PropertyTypeToggle("_BorderFoamStrength", 0f, 100f)
			},
			{
				MaterialEditorProperties.LightingMultiplier,
				new PropertyTypeToggle("_LightingMultiplier", 0f, 100f)
			},
			{
				MaterialEditorProperties.EdgeThresholdMin,
				new PropertyTypeToggle("_EdgeThresholdMin", 0f, 100f)
			},
			{
				MaterialEditorProperties.EdgeThreshold,
				new PropertyTypeToggle("_EdgeThreshold", 0f, 100f)
			},
			{
				MaterialEditorProperties.EdgeSharpness,
				new PropertyTypeToggle("_EdgeSharpness", 0f, 100f)
			},
			{
				MaterialEditorProperties.Value,
				new PropertyTypeToggle("_Value", 0f, 100f)
			},
			{
				MaterialEditorProperties.Width,
				new PropertyTypeToggle("_Width", 0f, 100f)
			},
			{
				MaterialEditorProperties.EdgeWidth,
				new PropertyTypeToggle("_EdgeWidth", 0f, 100f)
			},
			{
				MaterialEditorProperties.BorderWidth,
				new PropertyTypeToggle("_BorderWidth", 0f, 100f)
			},
			{
				MaterialEditorProperties.Overlay1_ST,
				new PropertyTypeVector4("_Overlay1_ST")
			},
			{
				MaterialEditorProperties.Overlay2_ST,
				new PropertyTypeVector4("_Overlay2_ST")
			},
			{
				MaterialEditorProperties.OverlayShift,
				new PropertyTypeVector4("_OverlayShift")
			},
			{
				MaterialEditorProperties.OverlayAlpha,
				new PropertyTypeVector4("_OverlayAlpha")
			},
			{
				MaterialEditorProperties.StencilComp,
				new PropertyTypeToggle("_StencilComp", 0f, 100f)
			},
			{
				MaterialEditorProperties.Stencil,
				new PropertyTypeToggle("_Stencil", 0f, 100f)
			},
			{
				MaterialEditorProperties.StencilOp,
				new PropertyTypeToggle("_StencilOp", 0f, 100f)
			},
			{
				MaterialEditorProperties.StencilWriteMask,
				new PropertyTypeToggle("_StencilWriteMask", 0f, 100f)
			},
			{
				MaterialEditorProperties.StencilReadMask,
				new PropertyTypeToggle("_StencilReadMask", 0f, 100f)
			},
			{
				MaterialEditorProperties.ColorMask,
				new PropertyTypeToggle("_ColorMask", 0f, 100f)
			},
			{
				MaterialEditorProperties.UseUIAlphaClip,
				new PropertyTypeToggle("_UseUIAlphaClip", 0f, 100f)
			},
			{
				MaterialEditorProperties.AlphaRect,
				new PropertyTypeVector4("_AlphaRect")
			},
			{
				MaterialEditorProperties.EnableSIG,
				new PropertyTypeToggle("_EnableSIG", 0f, 100f)
			},
			{
				MaterialEditorProperties.CapBorderBlendRange,
				new PropertyTypeToggle("_CapBorderBlendRange", 0f, 1f)
			},
			{
				MaterialEditorProperties.CapBorderBlendOffset,
				new PropertyTypeToggle("_CapBorderBlendOffset", -1f, 0f)
			},
			{
				MaterialEditorProperties.CapBorderBlendAngle,
				new PropertyTypeToggle("_CapBorderBlendAngle", 0.5f, 5f)
			},
			{
				MaterialEditorProperties.MainScrollSpeed,
				new PropertyTypeVector4("_MainScrollSpeed")
			},
			{
				MaterialEditorProperties.DetailScrollSpeed,
				new PropertyTypeVector4("_DetailScrollSpeed")
			},
			{
				MaterialEditorProperties.NervesScrollSpeed,
				new PropertyTypeVector4("_NervesScrollSpeed")
			},
			{
				MaterialEditorProperties.RefractScrollSpeed,
				new PropertyTypeVector4("_RefractScrollSpeed")
			},
			{
				MaterialEditorProperties.FlowIntensity,
				new PropertyTypeToggle("_FlowIntensity", 0f, 100f)
			},
			{
				MaterialEditorProperties.AlphaScale,
				new PropertyTypeToggle("_AlphaScale", 0f, 100f)
			},
			{
				MaterialEditorProperties.Angle,
				new PropertyTypeToggle("_Angle", 0f, 100f)
			},
			{
				MaterialEditorProperties.GlitchFrequency,
				new PropertyTypeToggle("_GlitchFrequency", 0f, 100f)
			},
			{
				MaterialEditorProperties.GlitchHeight,
				new PropertyTypeToggle("_GlitchHeight", 1E-06f, 0.2f)
			},
			{
				MaterialEditorProperties.GlitchSpeed,
				new PropertyTypeToggle("_GlitchSpeed", 0f, 100f)
			},
			{
				MaterialEditorProperties.GlitchColorIntensity,
				new PropertyTypeToggle("_GlitchColorIntensity", 0f, 100f)
			},
			{
				MaterialEditorProperties.GlitchOffsetIntensity,
				new PropertyTypeToggle("_GlitchOffsetIntensity", 0f, 100f)
			},
			{
				MaterialEditorProperties.OverlayIntensity,
				new PropertyTypeToggle("_OverlayIntensity", 0f, 100f)
			},
			{
				MaterialEditorProperties.UseBackground,
				new PropertyTypeToggle("_UseBackground", 0f, 100f)
			},
			{
				MaterialEditorProperties.Fill,
				new PropertyTypeToggle("_Fill", 0f, 100f)
			},
			{
				MaterialEditorProperties.Subdivide,
				new PropertyTypeToggle("_Subdivide", 0f, 100f)
			},
			{
				MaterialEditorProperties.Subdivisions,
				new PropertyTypeToggle("_Subdivisions", 0f, 20f)
			},
			{
				MaterialEditorProperties.SeparatorWidth,
				new PropertyTypeToggle("_SeparatorWidth", 0f, 100f)
			},
			{
				MaterialEditorProperties.SeparatorSmooth,
				new PropertyTypeToggle("_SeparatorSmooth", 0f, 0.999999f)
			},
			{
				MaterialEditorProperties.Shear,
				new PropertyTypeToggle("_Shear", 0f, 100f)
			},
			{
				MaterialEditorProperties.ShearTop,
				new PropertyTypeToggle("_ShearTop", 0f, 100f)
			},
			{
				MaterialEditorProperties.ShearBottom,
				new PropertyTypeToggle("_ShearBottom", 0f, 100f)
			},
			{
				MaterialEditorProperties.TileX,
				new PropertyTypeToggle("_TileX", 0f, 100f)
			},
			{
				MaterialEditorProperties.TileY,
				new PropertyTypeToggle("_TileY", 0f, 100f)
			},
			{
				MaterialEditorProperties.SrcFactor,
				new PropertyTypeToggle("_SrcFactor", 0f, 100f)
			},
			{
				MaterialEditorProperties.DstFactor,
				new PropertyTypeToggle("_DstFactor", 0f, 100f)
			},
			{
				MaterialEditorProperties.Chroma,
				new PropertyTypeToggle("_Chroma", 0f, 1f)
			},
			{
				MaterialEditorProperties.NotificationStrength,
				new PropertyTypeToggle("_NotificationStrength", 0f, 100f)
			},
			{
				MaterialEditorProperties.FillRect,
				new PropertyTypeVector4("_FillRect")
			},
			{
				MaterialEditorProperties.FillValue,
				new PropertyTypeToggle("_FillValue", 0f, 100f)
			},
			{
				MaterialEditorProperties.Slice9Grid,
				new PropertyTypeToggle("_Slice9Grid", 0f, 100f)
			},
			{
				MaterialEditorProperties.Size,
				new PropertyTypeVector4("_Size")
			},
			{
				MaterialEditorProperties.Radius,
				new PropertyTypeToggle("_Radius", 0f, 100f)
			},
			{
				MaterialEditorProperties.SrcFactorA,
				new PropertyTypeToggle("_SrcFactorA", 0f, 100f)
			},
			{
				MaterialEditorProperties.DstFactorA,
				new PropertyTypeToggle("_DstFactorA", 0f, 100f)
			},
			{
				MaterialEditorProperties.AlphaPremultiply,
				new PropertyTypeToggle("_AlphaPremultiply", 0f, 100f)
			},
			{
				MaterialEditorProperties.Cull,
				new PropertyTypeToggle("_Cull", 0f, 100f)
			},
			{
				MaterialEditorProperties.RayDir,
				new PropertyTypeVector4("_RayDir")
			},
			{
				MaterialEditorProperties.Length,
				new PropertyTypeToggle("_Length", 0f, 100f)
			},
			{
				MaterialEditorProperties.DetailsOnly,
				new PropertyTypeToggle("_DetailsOnly", 0f, 100f)
			},
			{
				MaterialEditorProperties.RimColor,
				new PropertyTypeColor("_RimColor")
			},
			{
				MaterialEditorProperties.MainSpeed,
				new PropertyTypeVector4("_MainSpeed")
			},
			{
				MaterialEditorProperties.MainOffset,
				new PropertyTypeToggle("_MainOffset", 0f, 100f)
			},
			{
				MaterialEditorProperties.DetailsSpeed,
				new PropertyTypeVector4("_DetailsSpeed")
			},
			{
				MaterialEditorProperties.DeformNormalStrength,
				new PropertyTypeToggle("_DeformNormalStrength", 0f, 100f)
			},
			{
				MaterialEditorProperties.SinWaveFrequency,
				new PropertyTypeVector4("_SinWaveFrequency")
			},
			{
				MaterialEditorProperties.SinWaveSpeed,
				new PropertyTypeVector4("_SinWaveSpeed")
			},
			{
				MaterialEditorProperties.SinWaveStrength,
				new PropertyTypeVector4("_SinWaveStrength")
			},
			{
				MaterialEditorProperties.VertexStrength,
				new PropertyTypeToggle("_VertexStrength", 0f, 100f)
			},
			{
				MaterialEditorProperties.RipplesAnimParams,
				new PropertyTypeVector4("_RipplesAnimParams")
			},
			{
				MaterialEditorProperties.RippleParams,
				new PropertyTypeVector4("_RippleParams")
			},
			{
				MaterialEditorProperties.RipplePos1,
				new PropertyTypeVector4("_RipplePos1")
			},
			{
				MaterialEditorProperties.MovementDir,
				new PropertyTypeVector4("_MovementDir")
			},
			{
				MaterialEditorProperties.Strech,
				new PropertyTypeToggle("_Strech", 0f, 100f)
			},
			{
				MaterialEditorProperties.EnableFXMesmer,
				new PropertyTypeToggle("_EnableFXMesmer", 0f, 100f)
			},
			{
				MaterialEditorProperties.EnableFXPropCannon,
				new PropertyTypeToggle("_EnableFXPropCannon", 0f, 100f)
			},
			{
				MaterialEditorProperties.AlphaFade,
				new PropertyTypeToggle("_AlphaFade", 0f, 1f)
			},
			{
				MaterialEditorProperties.MainScale,
				new PropertyTypeToggle("_MainScale", 0f, 100f)
			},
			{
				MaterialEditorProperties.PushStrength,
				new PropertyTypeToggle("_PushStrength", 0f, 100f)
			},
			{
				MaterialEditorProperties.Ztest,
				new PropertyTypeToggle("_Ztest", 0f, 100f)
			},
			{
				MaterialEditorProperties.NormalStrength,
				new PropertyTypeToggle("_NormalStrength", 0f, 100f)
			},
			{
				MaterialEditorProperties.NormalMap_Speed,
				new PropertyTypeVector4("_NormalMap_Speed")
			},
			{
				MaterialEditorProperties.NormalMap_TriplanarUV,
				new PropertyTypeVector4("_NormalMap_TriplanarUV")
			},
			{
				MaterialEditorProperties.MainTex_TriplanarUV,
				new PropertyTypeVector4("_MainTex_TriplanarUV")
			},
			{
				MaterialEditorProperties.MainTex2_TriplanarUV,
				new PropertyTypeVector4("_MainTex2_TriplanarUV")
			},
			{
				MaterialEditorProperties.DeformMap_TriplanarUV,
				new PropertyTypeVector4("_DeformMap_TriplanarUV")
			},
			{
				MaterialEditorProperties.RefractMap_TriplanarUV,
				new PropertyTypeVector4("_RefractMap_TriplanarUV")
			},
			{
				MaterialEditorProperties.sunSensitivity,
				new PropertyTypeToggle("_sunSensitivity", 0f, 100f)
			},
			{
				MaterialEditorProperties.Offset,
				new PropertyTypeToggle("_Offset", 0f, 1f)
			},
			{
				MaterialEditorProperties.SIGstr,
				new PropertyTypeVector4("_SIGstr")
			},
			{
				MaterialEditorProperties.ObjectRight,
				new PropertyTypeVector4("_ObjectRight")
			},
			{
				MaterialEditorProperties.WorldWaveDir,
				new PropertyTypeVector4("_WorldWaveDir")
			},
			{
				MaterialEditorProperties.WaveAmount,
				new PropertyTypeToggle("_WaveAmount", 0f, 100f)
			},
			{
				MaterialEditorProperties.WaveSpeed,
				new PropertyTypeToggle("_WaveSpeed", 0f, 1f)
			},
			{
				MaterialEditorProperties.TimeOffset,
				new PropertyTypeToggle("_TimeOffset", 0f, 100f)
			},
			{
				MaterialEditorProperties.BotColor,
				new PropertyTypeColor("_BotColor")
			},
			{
				MaterialEditorProperties.BotColor2,
				new PropertyTypeColor("_BotColor2")
			},
			{
				MaterialEditorProperties.GradientParams,
				new PropertyTypeVector4("_GradientParams")
			},
			{
				MaterialEditorProperties.MaskScale,
				new PropertyTypeToggle("_MaskScale", 0f, 100f)
			},
			{
				MaterialEditorProperties.MaskStr,
				new PropertyTypeToggle("_MaskStr", 0f, 100f)
			},
			{
				MaterialEditorProperties.ForceNormals,
				new PropertyTypeToggle("_ForceNormals", 0f, 1f)
			},
			{
				MaterialEditorProperties.CapColor,
				new PropertyTypeColor("_CapColor")
			},
			{
				MaterialEditorProperties.CapSpecColor,
				new PropertyTypeColor("_CapSpecColor")
			},
			{
				MaterialEditorProperties.CapEmissionScale,
				new PropertyTypeToggle("_CapEmissionScale", 0f, 2f)
			},
			{
				MaterialEditorProperties.SideEmissionScale,
				new PropertyTypeToggle("_SideEmissionScale", 0f, 2f)
			},
			{
				MaterialEditorProperties.CapScale,
				new PropertyTypeToggle("_CapScale", 0f, 100f)
			},
			{
				MaterialEditorProperties.SideScale,
				new PropertyTypeToggle("_SideScale", 0f, 100f)
			},
			{
				MaterialEditorProperties.TriplanarBlendRange,
				new PropertyTypeToggle("_TriplanarBlendRange", 0.1f, 80f)
			},
			{
				MaterialEditorProperties.InnerBorderBlendRange,
				new PropertyTypeToggle("_InnerBorderBlendRange", 0f, 1f)
			},
			{
				MaterialEditorProperties.InnerBorderBlendOffset,
				new PropertyTypeToggle("_InnerBorderBlendOffset", 0f, 1f)
			},
			{
				MaterialEditorProperties.Gloss,
				new PropertyTypeToggle("_Gloss", 0f, 1f)
			},
			{
				MaterialEditorProperties.EmissionScale,
				new PropertyTypeToggle("_EmissionScale", 0f, 2f)
			},
			{
				MaterialEditorProperties.PixelSnap,
				new PropertyTypeToggle("PixelSnap", 0f, 100f)
			},
			{
				MaterialEditorProperties.BorderTint,
				new PropertyTypeColor("_BorderTint")
			},
			{
				MaterialEditorProperties.BorderBlendRange,
				new PropertyTypeToggle("_BorderBlendRange", 0f, 1f)
			},
			{
				MaterialEditorProperties.BorderBlendOffset,
				new PropertyTypeToggle("_BorderBlendOffset", 0f, 1f)
			},
			{
				MaterialEditorProperties.BlendSrcFactor,
				new PropertyTypeToggle("_BlendSrcFactor", 0f, 100f)
			},
			{
				MaterialEditorProperties.BlendDstFactor,
				new PropertyTypeToggle("_BlendDstFactor", 0f, 100f)
			},
			{
				MaterialEditorProperties.IsOpaque,
				new PropertyTypeToggle("_IsOpaque", 0f, 100f)
			},
			{
				MaterialEditorProperties.AlphaTestValue,
				new PropertyTypeToggle("_AlphaTestValue", 0f, 100f)
			},
			{
				MaterialEditorProperties.TriplanarScale,
				new PropertyTypeToggle("_TriplanarScale", 0f, 100f)
			},
			{
				MaterialEditorProperties.FlowAmount,
				new PropertyTypeToggle("_FlowAmount", 0f, 100f)
			},
			{
				MaterialEditorProperties.MaskSpeed,
				new PropertyTypeToggle("_MaskSpeed", 0f, 100f)
			},
			{
				MaterialEditorProperties.FlowScale,
				new PropertyTypeToggle("_FlowScale", 0f, 100f)
			},
			{
				MaterialEditorProperties.Ambient,
				new PropertyTypeColor("_Ambient")
			},
			{
				MaterialEditorProperties.AnimSpeed,
				new PropertyTypeToggle("_AnimSpeed", 0f, 100f)
			},
			{
				MaterialEditorProperties.AnimStrength,
				new PropertyTypeToggle("_AnimStrength", 0f, 1f)
			}
		};
	}
}