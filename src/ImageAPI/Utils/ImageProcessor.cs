using System.Numerics;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageAPI.Utils;

public class ImageProcessor
{
    private const string DefaultColorHex = "#FDFDFD";
    private readonly Font font;
    private readonly Image image;

    public ImageProcessor(Image image, Font font)
    {
        this.image = image;
        this.font = font;
    }

    public virtual Image DrawText(string text, PointF coordinates, Color? color = null)
    {
        color ??= Color.ParseHex(DefaultColorHex);
        var textOptions = CreateTextOptions(coordinates);

        //image.Mutate(x => x.DrawText(text, font, color, coordinates));

        return image;
    }

    public virtual Image DrawCurrencySymbol(string currencySymbol, PointF coordinates)
    {
        using (var image = new Image<Rgba32>(100, 100))
        {
            var rect = new Rectangle((int)coordinates.X, (int)coordinates.Y, 80, 20);
            var radius = 10; // This value controls the radius of the rounded corners

            image.Mutate(x => x.Fill(Color.White, rect));
            image.Mutate(x => x.Fill(Color.Black, new RoundedRect(rect, radius), new GraphicsOptions()));

            image.Save("output.png");
        }
        return image;
    }

    private static IPathCollection BuildCorners(int imageWidth, int imageHeight, float cornerRadius)
    {
        // First create a square
        var rect = new RectangularPolygon(-0.5f, -0.5f, cornerRadius, cornerRadius);

        // Then cut out of the square a circle so we are left with a corner
        IPath cornerTopLeft = rect.Clip(new EllipsePolygon(cornerRadius - 0.5f, cornerRadius - 0.5f, cornerRadius));

        // Corner is now a corner shape positions top left
        // let's make 3 more positioned correctly, we can do that by translating the original around the center of the image.

        float rightPos = imageWidth - cornerTopLeft.Bounds.Width + 1;
        float bottomPos = imageHeight - cornerTopLeft.Bounds.Height + 1;

        // Move it across the width of the image - the width of the shape
        IPath cornerTopRight = cornerTopLeft.RotateDegree(90).Translate(rightPos, 0);
        IPath cornerBottomLeft = cornerTopLeft.RotateDegree(-90).Translate(0, bottomPos);
        IPath cornerBottomRight = cornerTopLeft.RotateDegree(180).Translate(rightPos, bottomPos);

        return new PathCollection(cornerTopLeft, cornerBottomLeft, cornerTopRight, cornerBottomRight);
    }

    private TextOptions CreateTextOptions(PointF coordinates)
    {
        return new TextOptions(font)
        {
            Origin = coordinates,
            TabWidth = 4,
            HorizontalAlignment = HorizontalAlignment.Left
        };
    }
}