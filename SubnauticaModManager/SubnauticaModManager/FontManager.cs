using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubnauticaModManager;

internal static class FontManager
{
    public static Dictionary<FontType, KnownFont> KnownFonts
    {
        get
        {
            if (_knownFonts == null) RefreshFonts();
            return _knownFonts;
        }
    }

    private static Dictionary<FontType, KnownFont> _knownFonts;

    public static KnownFont GetFont(FontType type)
    {
        if (KnownFonts.TryGetValue(type, out var font))
        {
            return font;
        }
        Plugin.Logger.LogError($"Could not find font of type {type}!");
        return null;
    }

    public static TMP_FontAsset GetFontAsset(FontType type)
    {
        return GetFont(type).asset;
    }

    private static void RefreshFonts()
    {
        _knownFonts = new Dictionary<FontType, KnownFont>();
        RegisterFont(FontType.Aller_Rg);

        var tmpFontAssets = Resources.FindObjectsOfTypeAll<TMP_FontAsset>();
        foreach (var fontAsset in tmpFontAssets)
        {
            foreach (var knownFont in _knownFonts.Values)
            {
                if (knownFont.FontAssetName == fontAsset.name)
                {
                    knownFont.asset = fontAsset;
                }
            }
        }
    }

    private static void RegisterFont(FontType type)
    {
        _knownFonts.Add(type, new KnownFont(type));
    }

    public class KnownFont
    {
        public readonly FontType type;
        public TMP_FontAsset asset;

        public KnownFont(FontType type)
        {
            this.type = type;
        }

        public string FontAssetName => type.ToString();
    }

    public enum FontType // must be the same as the 'name' property of the font asset
    {
        Aller_Rg
    }
}