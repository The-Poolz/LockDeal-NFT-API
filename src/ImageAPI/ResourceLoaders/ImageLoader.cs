using SixLabors.ImageSharp;

namespace ImageAPI.ResourceLoaders;

public sealed class ImageLoader : ResourceLoader<Image>
{
    public override Image Resource { get; }

    public ImageLoader()
        : base("ImageAPI.Resources.background.png")
    {
        Resource = Load();
    }

    public override Image Load()
    {
        using var imageStream = ResourceStream();
        return Image.Load(imageStream);
    }
}