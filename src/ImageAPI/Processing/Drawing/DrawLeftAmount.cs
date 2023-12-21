using SixLabors.Fonts;
using SixLabors.ImageSharp;
using static ImageAPI.Processing.DrawingSettings;

namespace ImageAPI.Processing.Drawing;

public class DrawLeftAmount : ToDrawing
{
    private readonly string leftAmount;

    public DrawLeftAmount(object leftAmount)
    {
        this.leftAmount = TruncateFractionalPart(leftAmount.ToString()!);
    }

    public override Image Draw(Image drawOn)
    {
        drawOn = Draw(drawOn, new ToDrawParameters("Left Amount", LeftAmount.HeaderPosition, HeaderFontSize));
        return Draw(drawOn, new ToDrawParameters(leftAmount, LeftAmount.Position, LeftAmount.FontSize, 5.5f, FontStyle.Bold));
    }

    private static string TruncateFractionalPart(string number)
    {
        var dotIndex = number.IndexOf('.');
        if (dotIndex != -1 && number.Length > dotIndex + 3)
        {
            return number[..(dotIndex + 3)] + "...";
        }
        return number;
    }
}