using System.Collections.Generic;
using UnityEngine;

namespace InventoryColorCustomization
{
    internal static class ColorChoiceManager
    {
        public static void Initialize()
        {
            ColorChoices = new List<ColorChoice>(_defaultColorChoices);
            foreach (var loadedBackground in CustomColorChoiceManager.loadedBackgrounds)
            {
                ColorChoices.Add(new CustomColorChoice(loadedBackground));
            }
            _initialized = true;
        }

        public static List<ColorChoice> ColorChoices;

        private static bool _initialized = false;

        private static ColorChoice[] _defaultColorChoices = new ColorChoice[]
        {
            new DefaultColorChoice(),
            new ColorChoice("Red", new Color(242f / 255, 68f / 255, 68f / 255)),
            new ColorChoice("Orange", new Color(252f / 255, 82f / 255, 3f / 255)),
            new ColorChoice("Yellow", new Color(252f / 255, 227f / 255, 3f / 255)),
            new ColorChoice("Green", new Color(88f / 255, 232f / 255, 84f / 255)),
            new ColorChoice("Dark Green", new Color(30f / 255, 134f / 255, 40f / 255)),
            new ColorChoice("Cyan", new Color(0f, 0.83f, 0.99f)),
            new ColorChoice("Purple", new Color(179f / 255, 106f / 255, 247f / 255)),
            new ColorChoice("White", new Color(1, 1, 1)),
            new ColorChoice("Gray", new Color(0.5f, 0.5f, 0.5f)),
        };

        public static void RefreshAll()
        {
            if (!_initialized)
            {
                return;
            }
            foreach (var choice in ColorChoices)
            {
                choice.RefreshSprite();
            }
        }

        public static ColorChoice GetColorChoiceAtIndex(int index)
        {
            return ColorChoices[index];
        }

        public static string[] GetColorChoiceNames(BackgroundType backgroundType)
        {
            var colorChoicesAsString = new string[ColorChoices.Count];
            for (int i = 0; i < ColorChoices.Count; i++)
            {
                colorChoicesAsString[i] = ColorChoices[i].GetColoredNameString(backgroundType);
            }
            return colorChoicesAsString;
        }

        // ModOptions class is extremely limited, that is why I HAVE to do this
        public static int ChoiceNameToIndex(string backgroundDataID, string choiceOption)
        {
            foreach (var backgroundData in BackgroundDataManager.BackgroundsDatas)
            {
                if (backgroundData.ID == backgroundDataID)
                {
                    for (int i = 0; i < ColorChoices.Count; i++)
                    {
                        if (ColorChoices[i].GetColoredNameString(backgroundData.BackgroundType) == choiceOption)
                        {
                            return i;
                        }
                    }
                }
            }
            return -1;
        }
    }
}
