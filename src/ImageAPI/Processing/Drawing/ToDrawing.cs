using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace ImageAPI.Processing.Drawing;

public abstract class ToDrawing
{
    public abstract string Text { get; }
    public abstract Font Font { get; }
    public abstract PointF Coordinates { get; }

    public Image Draw(Image drawOn)
    {

    }
}