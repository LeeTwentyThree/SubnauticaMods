using UnityEngine;
using SMLHelper.V2.Utility;
using System.Collections.Generic;

namespace InventoryColorCustomization
{
    internal static class BackgroundDataManager
    {
        public static BackgroundData[] BackgroundsDatas = new BackgroundData[]
        {
            new BackgroundData(new BackgroundType(CraftData.BackgroundType.Normal), "Normal"),
            new BackgroundData(new BackgroundType(CraftData.BackgroundType.Blueprint), "Blueprint"),
            new BackgroundData(new BackgroundType(CraftData.BackgroundType.PlantWater), "PlantWater"),
            new BackgroundData(new BackgroundType(CraftData.BackgroundType.PlantWaterSeed), "PlantWater"),
            new BackgroundData(new BackgroundType(CraftData.BackgroundType.PlantAir), "PlantAir"),
            new BackgroundData(new BackgroundType(CraftData.BackgroundType.PlantAirSeed), "PlantAir"),
            new BackgroundData(new BackgroundType(CraftData.BackgroundType.ExosuitArm), "ExosuitArm"),

            new BackgroundData(new BackgroundType(BackgroundTypeManager.Category_Creatures), "Normal"),
            new BackgroundData(new BackgroundType(BackgroundTypeManager.Category_Precursor), "Normal"),
            new BackgroundData(new BackgroundType(BackgroundTypeManager.Category_Tools), "Normal"),
            new BackgroundData(new BackgroundType(BackgroundTypeManager.Category_Deployables), "Normal"),
            new BackgroundData(new BackgroundType(BackgroundTypeManager.Category_FoodDrinks), "Normal"),
        };

        public static BackgroundData GetDefaultBackgroundData()
        {
            return BackgroundsDatas[0];
        }

        public static BackgroundData GetBackgroundData(BackgroundType backgroundType)
        {
            foreach (var background in BackgroundsDatas)
            {
                if (background.BackgroundType.Equals(backgroundType))
                {
                    return background;
                }
            }
            return null;
        }

        public static void RefreshAll()
        {
            foreach (var data in BackgroundsDatas)
            {
                data.RefreshDefaultSprite();
            }
        }
    }
}
