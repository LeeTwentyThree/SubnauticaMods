using UnityEngine;
using System.IO;

namespace InventoryColorCustomization
{
    // data related to an item background, including default textures (if any)
    internal class BackgroundData
    {
        public BackgroundType BackgroundType;

        public Texture2D DefaultTexture { get; private set; }

        public Atlas.Sprite DefaultSprite { get; private set; }

        public string ID
        {
            get
            {
                if (BackgroundType.UseEnum)
                {
                    return BackgroundType.enumValue.ToString() + "Color";
                }
                else
                {
                    return BackgroundType.stringValue + "Color";
                }
            }
        }

        public BackgroundData(BackgroundType backgroundType, string defaultTextureFileName = null)
        {
            BackgroundType = backgroundType;
            if (!string.IsNullOrEmpty(defaultTextureFileName))
            {
                DefaultTexture = BackgroundIconGenerator.LoadTextureFromFile(Main.GetPathInAssetsFolder(Path.Combine("Default", defaultTextureFileName + ".png")));
            }
            RefreshDefaultSprite();
        }

        public void RefreshDefaultSprite()
        {
            DefaultSprite = BackgroundIconGenerator.TextureToBGSprite(DefaultTexture);
        }
    }
}
