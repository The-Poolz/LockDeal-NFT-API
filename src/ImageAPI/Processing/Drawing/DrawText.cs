using ImageAPI.Processing.Drawing.Options;
using SixLabors.ImageSharp;
using static ImageAPI.Settings.DrawingSettings;

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

    public override void Draw(Image drawOn)
    {
        Draw(drawOn, new DrawOptions(text, coordinates, HeaderFontSize));
    }
}