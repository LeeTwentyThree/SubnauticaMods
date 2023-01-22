using UnityEngine;

namespace InventoryColorCustomization
{
    internal class CustomColorChoice : ColorChoice
    {
        public CustomColorChoice(CustomBackground customBackground) : base(customBackground.name + " (Custom)", Color.white)
        {
            this.customBackground = customBackground;
        }

        private CustomBackground customBackground;

        public override Atlas.Sprite GetSprite(BackgroundType backgroundType)
        {
            if (cachedSprite == null)
            {
                cachedSprite = BackgroundIconGenerator.TextureToBGSprite(customBackground.texture);
            }
            return cachedSprite;
        }

        public override Color TextColor(BackgroundType backgroundType)
        {
            return BackgroundIconGenerator.GetRepresentationalColor(GetSprite(backgroundType).texture);
        }
    }
}
