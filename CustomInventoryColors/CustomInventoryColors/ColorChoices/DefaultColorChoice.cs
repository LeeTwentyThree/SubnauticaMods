using UnityEngine;
using System.Collections.Generic;

namespace InventoryColorCustomization
{
    // this class defines the default color choice for ALL background types!
    internal class DefaultColorChoice : ColorChoice
    {
        public DefaultColorChoice() : base("Default", Color.white)
        {
        }

        public override string GetName(BackgroundType backgroundType)
        {
            if (backgroundType.UseEnum)
            {
                switch (backgroundType.enumValue)
                {
                    case CraftData.BackgroundType.Normal:
                        return "Default (blue)";
                    case CraftData.BackgroundType.Blueprint:
                        return "Default (purple)";
                    case CraftData.BackgroundType.PlantWater:
                        return "Default (blue)";
                    case CraftData.BackgroundType.PlantWaterSeed:
                        return "Default (blue)";
                    case CraftData.BackgroundType.PlantAir:
                        return "Default (green)";
                    case CraftData.BackgroundType.PlantAirSeed:
                        return "Default (green)";
                    case CraftData.BackgroundType.ExosuitArm:
                        return "Default (purple)";
                    default:
                        return "Unknown";
                }
            }
            else
            {
                switch (backgroundType.stringValue)
                {
                    default:
                        return GetColoredNameString(new BackgroundType(CraftData.BackgroundType.Normal));
                }
            }
        }

        private Atlas.Sprite defaultSprite; // sprite used for "normal items"
        private Dictionary<CraftData.BackgroundType, Atlas.Sprite> backgroundSprites = new Dictionary<CraftData.BackgroundType, Atlas.Sprite>(); // default background sprites for all of the vanilla background types

        private void CacheBackgroundSprite(CraftData.BackgroundType type, Atlas.Sprite sprite)
        {
            if (!backgroundSprites.ContainsKey(type))
            {
                backgroundSprites.Add(type, sprite);
                return;
            }
            backgroundSprites[type] = sprite;
        }

        private bool BackgroundSpriteCached(CraftData.BackgroundType type)
        {
            return backgroundSprites.ContainsKey(type);
        }

        private Atlas.Sprite GetBackgroundSprite(CraftData.BackgroundType type)
        {
            if (backgroundSprites.TryGetValue(type, out var sprite))
            {
                return sprite;
            }
            return null;
        }

        public override Atlas.Sprite GetSprite(BackgroundType backgroundType)
        {
            if (backgroundType.VanillaBackground)
            {
                var vanillaBackgroundType = backgroundType.enumValue;
                if (!BackgroundSpriteCached(vanillaBackgroundType))
                {
                    CacheBackgroundSprite(vanillaBackgroundType, BackgroundDataManager.GetBackgroundData(backgroundType).DefaultSprite);
                }
                return GetBackgroundSprite(vanillaBackgroundType);
            }
            else if (defaultSprite == null)
            {
                defaultSprite = BackgroundDataManager.GetDefaultBackgroundData().DefaultSprite;
            }
            return defaultSprite;
        }

        public override void RefreshSprite()
        {
            backgroundSprites = new Dictionary<CraftData.BackgroundType, Atlas.Sprite>();
            defaultSprite = null;
        }

        public override Color TextColor(BackgroundType backgroundType)
        {
            return Color.white;
        }
    }
}
