using SixLabors.ImageSharp;

namespace ImageAPI.Processing.Drawing;

public class DrawLeftAmount : ToDrawing
{
    private readonly string leftAmount;

    public DrawLeftAmount(object leftAmount)
    {
        this.leftAmount = leftAmount.ToString()!;
    }

    public override Image Draw(Image drawOn)
    {
        drawOn = Draw(drawOn, "Left Amount", new PointF(ImageSize.Width - 400, ImageSize.Height - 330), TextFontSize);
        return Draw(drawOn, leftAmount, new PointF(ImageSize.Width - 400, ImageSize.Height - 290), AmountFontSize);
    }
}