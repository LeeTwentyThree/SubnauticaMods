using UnityEngine;

namespace InventoryColorCustomization
{
    internal class ColorChoice
    {
        protected string name;
        protected Color color;

        protected Atlas.Sprite cachedSprite;

        public ColorChoice(string name, Color color)
        {
            this.name = name;
            this.color = color;
        }

        public virtual string GetName(BackgroundType backgroundType)
        {
            return name;
        }

        public string GetColoredNameString(BackgroundType backgroundType)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(TextColor(backgroundType))}>{GetName(backgroundType)}</color>";
        }

        public virtual Atlas.Sprite GetSprite(BackgroundType backgroundType)
        {
            if (cachedSprite == null)
            {
                cachedSprite = BackgroundIconGenerator.CreatePaintedIcon(color);
            }
            return cachedSprite;
        }

        // call this method when the sprite of a color choice needs to be changed
        public virtual void RefreshSprite()
        {
            cachedSprite = null;
        }

        public virtual Color TextColor(BackgroundType backgroundType)
        {
            return color;
        }
    }
}
