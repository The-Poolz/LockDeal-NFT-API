using SixLabors.ImageSharp;

namespace ImageAPI.Processing;

public static class ImageSize
{
    public static int Width { get; private set; }
    public static int Height { get; private set; }

    public static void Initialize(Image image)
    {
        Width = image.Width;
        Height = image.Height;
    }
}