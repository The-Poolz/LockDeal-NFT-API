using SixLabors.Fonts;

namespace ImageAPI.ResourceLoaders;

public sealed class FontLoader : ResourceLoader<FontFamily>
{
    public override FontFamily Resource { get; }

    public FontLoader()
        : base("ImageAPI.Resources.ABCNormal.otf")
    {
        Resource = Load();
    }

    public override FontFamily Load()
    {
        using var fontStream = ResourceStream();
        var fontCollection = new FontCollection();
        return fontCollection.Add(fontStream);
    }
}