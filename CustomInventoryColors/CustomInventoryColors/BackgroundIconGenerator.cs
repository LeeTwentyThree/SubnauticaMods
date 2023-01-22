using UnityEngine;
using SMLHelper.V2.Utility;
using System.IO;

namespace InventoryColorCustomization
{
    internal static class BackgroundIconGenerator
    {
        // grayscale texture that can be "painted" to your liking
        private static Texture2D ReferenceTexture
        {
            get
            {
                if (_referenceTexture == null)
                {
                    _referenceTexture = LoadTextureFromFile(Main.GetPathInAssetsFolder("Grayscale.png"));
                }
                return _referenceTexture;
            }
        }

        private static Texture2D _referenceTexture;

        public static Atlas.Sprite TextureToBGSprite(Texture2D texture)
        {
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;
            return new Atlas.Sprite(texture) { slice9Grid = !Main.modConfig.SquareIcons };
        }

        // ripped from SMLHelper, thanks modders!
        public static Texture2D LoadTextureFromFile(string filePathToImage)
        {
            if (File.Exists(filePathToImage))
            {
                byte[] array = File.ReadAllBytes(filePathToImage);
                Texture2D texture2D = new Texture2D(2, 2, TextureFormat, false, LinearColorSpace);
                ImageConversion.LoadImage(texture2D, array);
                return texture2D;
            }
            return null;
        }

        private static bool LinearColorSpace
        {
            get
            {
                return true;
            }
        }

        private static TextureFormat TextureFormat
        {
            get
            {
                return TextureFormat.BC7;
            }
        }

        public static Atlas.Sprite CreatePaintedIcon(Color color)
        {
            var grayscale = ReferenceTexture;
            return TextureToBGSprite(PaintGrayscaleTexture(grayscale, color));
        }

        private static Color PaintGrayPixel(Color gray, Color hue)
        {
            return new Color(gray.r * hue.r, gray.g * hue.g, gray.b * hue.b, gray.a * hue.a);
        }

        private static Texture2D PaintGrayscaleTexture(Texture2D grayscale, Color newColor) // give life to a grayscale image!!!
        {
            if (grayscale == null)
            {
                return null;
            }

            Texture2D newTex = new Texture2D(grayscale.width, grayscale.height, TextureFormat.ARGB32, false, LinearColorSpace);

            var fillPixels = new Color[newTex.width * newTex.height];

            for (int i = 0; i < fillPixels.Length; i++)
            {
                fillPixels[i] = Color.clear;
            }

            newTex.SetPixels(fillPixels);

            for (int x = 0; x < grayscale.width; x++)
            {
                for (int y = 0; y < grayscale.height; y++)
                {
                    newTex.SetPixel(x, y, PaintGrayPixel(grayscale.GetPixel(x, y), newColor));
                }
            }

            newTex.Apply();

            return newTex;
        }

        public static Color GetRepresentationalColor(Texture2D texture)
        {
            Color pixel = texture.GetPixel(0, 0);
            if (texture.height > 3)
            {
                var verticalPositionToCheck = texture.height - 3;
                pixel = texture.GetPixel(0, verticalPositionToCheck);
            }
            return new Color(pixel.r, pixel.g, pixel.b, 1f);
        }
    }
}