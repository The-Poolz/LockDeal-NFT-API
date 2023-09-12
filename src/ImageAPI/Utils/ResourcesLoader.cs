using SixLabors.Fonts;
using System.Reflection;
using SixLabors.ImageSharp;

namespace ImageAPI.Utils;

public static class ResourcesLoader
{
    private const string BackgroundResourceName = "ImageAPI.Resources.Sample-png-Image-for-Testing.png";
    private const string FontResourceName = "ImageAPI.Resources.LEMONMILK-Regular.otf";

    public static Image LoadImageFromEmbeddedResources()
    {
        using var imageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(BackgroundResourceName);

        return imageStream == null
            ? throw new FileNotFoundException($"Could not find the embedded resource '{BackgroundResourceName}'. Make sure the resource exists and its 'Build Action' is set to 'Embedded Resource'.")
            : Image.Load(imageStream);
    }

    public static Font LoadFontFromEmbeddedResources(float size = 24f)
    {
        using var fontStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(FontResourceName) ??
                               throw new FileNotFoundException($"Could not find the embedded resource '{FontResourceName}'. Make sure the resource exists and its 'Build Action' is set to 'Embedded Resource'.");

        var fontCollection = new FontCollection();
        var fontFamily = fontCollection.Add(fontStream);
        return new Font(fontFamily, size);
    }
}