using HarmonyLib;
using UnityEngine.UI;

namespace InventoryColorCustomization.Patches
{
    [HarmonyPatch(typeof(SpriteManager))]
    public static class SpriteManagerPatches
    {
        // public static Atlas.Sprite GetBackground(CraftData.BackgroundType backgroundType)
        [HarmonyPatch(nameof(SpriteManager.GetBackground))] // original method gets an Atlas.Sprite for the item background
        [HarmonyPatch(new System.Type[] { typeof(TechType) })] // clarify the correct method overload
        [HarmonyPrefix()]
        public static bool SetBackgroundSpritePostfix(TechType techType, ref Atlas.Sprite __result)
        {
            var actualBackgroundType = BackgroundTypeManager.GetBackgroundType(techType);
            var backgroundData = actualBackgroundType.GetData();
            if (backgroundData == null)
            {
                return true;
            }
            var backgroundDataId = backgroundData.ID;
            var choiceIndex = Main.modConfig.GetSelectedIndexForBackground(backgroundDataId);
            if (choiceIndex < 0) // if invalid (-1), use original background
            {
                return true;
            }
            var choice = ColorChoiceManager.GetColorChoiceAtIndex(choiceIndex);
            __result = choice.GetSprite(actualBackgroundType);
            return false;
        }
    }
}