using SixLabors.Fonts;

namespace ImageAPI.ResourceLoaders;

public sealed class FontFamilyLoader : ResourceLoader<FontFamily>
{
    public override FontFamily Resource { get; }

    public FontFamilyLoader()
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