using System.Numerics;
using SixLabors.Fonts;
using ImageAPI.Settings;
using SixLabors.ImageSharp;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ImageAPI.Processing;

public static class TextDrawer
{
    public static void DrawText(this Image drawOn, string text, PointF coordinates, float fontSize, FontStyle fontStyle = FontStyle.Regular, float penWidth = 2f, Color color = default)
    {
        color = color == default ? ColorPalette.White : color;
        var textOptions = new RichTextOptions(Resources.Font(fontSize, fontStyle))
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            Origin = new PointF(coordinates.X - penWidth, coordinates.Y - penWidth)
        };

        drawOn.Mutate(x => x.DrawText(
            textOptions,
            text,
            Brushes.Solid(color), 
            Pens.Solid(color, penWidth)
        ));
    }

    public static void DrawHeader(this Image drawOn, string text, PointF coordinates)
    {
        drawOn.DrawText(text, coordinates, 16f);
    }

    public static void DrawProviderName(this Image drawOn, string providerName)
    {
        drawOn.DrawText(Regex.Replace(providerName, "(?<!^)([A-Z])", " $1"), new PointF(672, 52), 24f);
    }

    public static void DrawLeftAmount(this Image drawOn, object leftAmount)
    {
        drawOn.DrawHeader("Left Amount", new PointF(672, 274));
        drawOn.DrawText(leftAmount.ToString()!, new PointF(672, 298), 64f, FontStyle.Bold, 5.5f);
    }

    public static void DrawPoolId(this Image drawOn, BigInteger poolId)
    {
        drawOn.DrawText($"#{poolId}", new PointF(981, 52), 24f);
    }
}