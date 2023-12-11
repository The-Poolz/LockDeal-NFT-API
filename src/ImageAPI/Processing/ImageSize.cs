using SixLabors.ImageSharp;

namespace ImageAPI.Processing;

public static class ImageSize
{
    private static bool isInitialized;

    public static int Width { get; private set; }
    public static int Height { get; private set; }

    public static void Initialize(Image image)
    {
        if (isInitialized)
        {
            throw new InvalidOperationException("Cannot initialize 'ImageSize' class twice.");
        }
        Width = image.Width;
        Height = image.Height;
        isInitialized = true;
    }
}