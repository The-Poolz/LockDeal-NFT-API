using SixLabors.ImageSharp;

namespace ImageAPI.Processing.Drawing;

public class DrawText : ToDrawing
{
    public DrawText(string text, float x, float y)
        : base(
            text: text,
            coordinates: new PointF(x, y),
            fontSize: TextFontSize
        )
    { }
}