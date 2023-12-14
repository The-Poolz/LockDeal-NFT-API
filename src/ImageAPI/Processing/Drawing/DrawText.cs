using SixLabors.ImageSharp;
using static ImageAPI.Processing.DrawingSettings;

namespace ImageAPI.Processing.Drawing;

public class DrawText : ToDrawing
{
    private readonly string text;
    private readonly PointF coordinates;

    public DrawText(string text, float x, float y)
    {
        this.text = text;
        coordinates = new PointF(x, y);
    }

    public override Image Draw(Image drawOn)
    {
        return Draw(drawOn, text, coordinates, HeaderFontSize);
    }
}