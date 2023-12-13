using SixLabors.ImageSharp;

namespace ImageAPI.Processing.Drawing;

public class DrawLeftAmount : ToDrawing
{
    public DrawLeftAmount(object leftAmount)
        : base(
            text: leftAmount.ToString()!,
            coordinates: new PointF(ImageSize.Width - 400, ImageSize.Height - 290),
            fontSize: AmountFontSize
        )
    { }
}