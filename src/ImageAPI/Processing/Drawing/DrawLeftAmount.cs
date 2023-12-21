using ImageAPI.Processing.Drawing.Options;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using static ImageAPI.Settings.DrawingSettings;

namespace ImageAPI.Processing.Drawing;

public class DrawLeftAmount : ToDrawing
{
    private readonly string leftAmount;

    public DrawLeftAmount(object leftAmount)
    {
        this.leftAmount = leftAmount.ToString()!;
    }

    public override void Draw(Image drawOn)
    {
        Draw(drawOn, new DrawOptions("Left Amount", LeftAmount.HeaderPosition, HeaderFontSize));
        Draw(drawOn, new DrawOptions(leftAmount, LeftAmount.Position, LeftAmount.FontSize, FontStyle.Bold, 5.5f));
    }
}