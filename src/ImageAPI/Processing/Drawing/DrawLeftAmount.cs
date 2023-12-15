using SixLabors.ImageSharp;
using static ImageAPI.Processing.DrawingSettings;

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
        drawOn = Draw(drawOn, "Left Amount", LeftAmount.HeaderPosition, HeaderFontSize);
        return Draw(drawOn, leftAmount, LeftAmount.Position, LeftAmount.FontSize);
    }
}