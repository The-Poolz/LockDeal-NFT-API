using System.Numerics;
using SixLabors.Fonts;
using ImageAPI.Settings;
using MetaDataAPI.Utils;
using SixLabors.ImageSharp;
using System.Globalization;
using SixLabors.ImageSharp.Drawing;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ImageAPI.Processing;

public static class ImageExtensions
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

    public static void DrawStartTime(this Image drawOn, object startTime)
    {
        drawOn.DrawHeader("Start Time", new PointF(48, 274));
        drawOn.DrawDate(startTime, new PointF(48, 298), 32f);
        drawOn.DrawTime(startTime, new PointF(48, 340), 32f);
    }

    public static void DrawFinishTime(this Image drawOn, object finishTime)
    {
        drawOn.DrawHeader("Finish Time", new PointF(276, 274));
        drawOn.DrawDate(finishTime, new PointF(276, 298), 32f);
        drawOn.DrawTime(finishTime, new PointF(276, 340), 32f);
    }

    public static void DrawDate(this Image drawOn, object dateTime, PointF coordinates, float fontSize)
    {
        drawOn.DrawText(TimeUtils.FromUnixTimestamp((long)dateTime).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), coordinates, fontSize);
    }

    public static void DrawTime(this Image drawOn, object dateTime, PointF coordinates, float fontSize)
    {
        drawOn.DrawText(TimeUtils.FromUnixTimestamp((long)dateTime).ToString("HH:mm:ss", CultureInfo.InvariantCulture), coordinates, fontSize);
    }

    public static void DrawPoolId(this Image drawOn, BigInteger poolId)
    {
        drawOn.DrawText($"#{poolId}", new PointF(981, 52), 24f);
    }

    public static void DrawTokenBadge(this Image drawOn, string currencyName)
    {
        drawOn.DrawCurrencyBadge(currencyName, new PointF(554, 270), 16f);
    }

    public static void DrawRefundBadge(this Image drawOn, string currencyName)
    {
        drawOn.DrawCurrencyBadge(currencyName, new PointF(554, 445), 16f);
    }

    public static void DrawCurrencyBadge(this Image drawOn, string currencyName, PointF coordinates, float fontSize, float penWidth = 2f)
    {
        const int widthPadding = 20;
        const int heightPadding = 8;

        var textOptions = new RichTextOptions(Resources.Font(fontSize))
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        var text = $"${currencyName}";
        var textSize = TextMeasurer.MeasureAdvance(text, textOptions);

        var rectWidth = textSize.Width + widthPadding;
        var rectHeight = textSize.Height + heightPadding;
        var rectangle = new RectangleF(
            coordinates.X - penWidth,
            coordinates.Y - penWidth,
            rectWidth,
            rectHeight
        );

        var cornerRadius = (textSize.Height + heightPadding) / 2;
        var roundedRectPath = CreateRoundedRectanglePath(rectangle, cornerRadius);

        drawOn.Mutate(x => x.Fill(ColorPalette.White, roundedRectPath));

        textOptions.Origin = new PointF(
            rectangle.Left - penWidth + (rectangle.Width / 2),
            rectangle.Top - penWidth + (rectangle.Height / 2)
        );
        drawOn.Mutate(x => x.DrawText(textOptions, text, Pens.Solid(ColorPalette.Black, penWidth)));
    }

    private static IPath CreateRoundedRectanglePath(RectangleF rect, float cornerRadius)
    {
        var topLeft = new PointF(rect.Left, rect.Top);
        var topRight = new PointF(rect.Right, rect.Top);
        var bottomRight = new PointF(rect.Right, rect.Bottom);
        var bottomLeft = new PointF(rect.Left, rect.Bottom);

        var pathBuilder = new PathBuilder();

        pathBuilder.MoveTo(new PointF(topLeft.X + cornerRadius, topLeft.Y));
        pathBuilder.LineTo(new PointF(topRight.X - cornerRadius, topRight.Y));

        var topRightCorner = new PointF(topRight.X - cornerRadius, topRight.Y + cornerRadius);
        pathBuilder.AddArc(topRightCorner, cornerRadius, cornerRadius, 0, 270, 90);

        pathBuilder.LineTo(new PointF(bottomRight.X, bottomRight.Y - cornerRadius));

        var bottomRightCorner = new PointF(bottomRight.X - cornerRadius, bottomRight.Y - cornerRadius);
        pathBuilder.AddArc(bottomRightCorner, cornerRadius, cornerRadius, 0, 0, 90);

        pathBuilder.LineTo(new PointF(bottomLeft.X + cornerRadius, bottomLeft.Y));

        var bottomLeftCorner = new PointF(bottomLeft.X + cornerRadius, bottomLeft.Y - cornerRadius);
        pathBuilder.AddArc(bottomLeftCorner, cornerRadius, cornerRadius, 0, 90, 90);

        pathBuilder.LineTo(new PointF(topLeft.X, topLeft.Y + cornerRadius));

        var topLeftCorner = new PointF(topLeft.X + cornerRadius, topLeft.Y + cornerRadius);
        pathBuilder.AddArc(topLeftCorner, cornerRadius, cornerRadius, 0, 180, 90);

        pathBuilder.CloseFigure();

        return pathBuilder.Build();
    }
}