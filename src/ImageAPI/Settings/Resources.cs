using SixLabors.Fonts;
using SixLabors.ImageSharp;
using ImageAPI.ResourceLoaders;

namespace ImageAPI.Settings;

public static class Resources
{
    private static readonly ImageLoader imageLoader = new();
    private static readonly FontFamilyLoader fontFamilyLoader = new();
    private static readonly FontFamily fontFamily = fontFamilyLoader.Resource;

    public static Image BackgroundImage => imageLoader.Resource;
    public static Font Font(float fontSize, FontStyle fontStyle = FontStyle.Regular) => new(fontFamily, fontSize, fontStyle);
}