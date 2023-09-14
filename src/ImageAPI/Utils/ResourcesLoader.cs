using SixLabors.Fonts;
using System.Reflection;
using SixLabors.ImageSharp;

namespace ImageAPI.Utils;

public class ResourcesLoader
{
    public const string BackgroundResourceName = "ImageAPI.Resources.Sample-png-Image-for-Testing.png";
    public const string FontResourceName = "ImageAPI.Resources.LEMONMILK-Regular.otf";

    public virtual Image LoadImageFromEmbeddedResources()
    {
        using var imageStream = GetStream(BackgroundResourceName) ??
            throw new FileNotFoundException($"Could not find the embedded resource '{BackgroundResourceName}'. Make sure the resource exists and its 'Build Action' is set to 'Embedded Resource'.");

        return Image.Load(imageStream);
    }

    public virtual Font LoadFontFromEmbeddedResources(float size = 24f)
    {
        using var fontStream = GetStream(FontResourceName) ??
            throw new FileNotFoundException($"Could not find the embedded resource '{FontResourceName}'. Make sure the resource exists and its 'Build Action' is set to 'Embedded Resource'.");

        var fontCollection = new FontCollection();
        var fontFamily = fontCollection.Add(fontStream);
        return new Font(fontFamily, size);
    }

    public virtual Stream? GetStream(string resourceName) =>
        Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
}