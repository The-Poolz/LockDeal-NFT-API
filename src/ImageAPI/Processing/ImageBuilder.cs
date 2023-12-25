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

public class ImageBuilder
{
    private readonly Image image;

    public ImageBuilder(Image image)
    {
        this.image = image;
    }

    public ImageBuilder DrawText(string text, PointF coordinates, float fontSize, FontStyle fontStyle = FontStyle.Regular, float penWidth = 2f, Color color = default)
    {
        color = color == default ? ColorPalette.White : color;
        var textOptions = new RichTextOptions(Resources.Font(fontSize, fontStyle))
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            Origin = new PointF(coordinates.X - penWidth, coordinates.Y - penWidth)
        };

        image.Mutate(x => x.DrawText(
            textOptions,
            text,
            Brushes.Solid(color),
            Pens.Solid(color, penWidth)
        ));
        return this;
    }

    public ImageBuilder DrawHeader(string text, PointF coordinates)
    {
        return DrawText(text, coordinates, 16f);
    }

    public ImageBuilder DrawProviderName(string providerName)
    {
        return DrawText(Regex.Replace(providerName, "(?<!^)([A-Z])", " $1"), new PointF(672, 52), 24f);
    }

    public ImageBuilder DrawLeftAmount(object leftAmount)
    {
        DrawHeader("Left Amount", new PointF(672, 274));
        return DrawText(leftAmount.ToString()!, new PointF(672, 298), 64f, FontStyle.Bold, 5.5f);
    }

    public ImageBuilder DrawPoolId(BigInteger poolId)
    {
        return DrawText($"#{poolId}", new PointF(981, 52), 24f);
    }

    public ImageBuilder DrawStartTime(object startTime)
    {
        DrawHeader("Start Time", new PointF(48, 274));
        DrawDate(startTime, new PointF(48, 298), 32f);
        return DrawTime(startTime, new PointF(48, 340), 32f);
    }

    public ImageBuilder DrawFinishTime(object finishTime)
    {
        DrawHeader("Finish Time", new PointF(276, 274));
        DrawDate(finishTime, new PointF(276, 298), 32f);
        return DrawTime(finishTime, new PointF(276, 340), 32f);
    }

    public ImageBuilder DrawDate(object dateTime, PointF coordinates, float fontSize)
    {
        return DrawText(TimeUtils.FromUnixTimestamp((long)dateTime).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), coordinates, fontSize);
    }

    public ImageBuilder DrawTime(object dateTime, PointF coordinates, float fontSize)
    {
        return DrawText(TimeUtils.FromUnixTimestamp((long)dateTime).ToString("HH:mm:ss", CultureInfo.InvariantCulture), coordinates, fontSize);
    }

    public ImageBuilder DrawTokenBadge(string currencyName)
    {
        return DrawCurrencyBadge(currencyName, new PointF(554, 270), 16f);
    }

    public ImageBuilder DrawRefundBadge(string currencyName)
    {
        return DrawCurrencyBadge(currencyName, new PointF(554, 445), 16f);
    }

    public ImageBuilder DrawCurrencyBadge(string currencyName, PointF coordinates, float fontSize, float penWidth = 2f)
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

        image.Mutate(x => x.Fill(ColorPalette.White, roundedRectPath));

        textOptions.Origin = new PointF(
            rectangle.Left - penWidth + (rectangle.Width / 2),
            rectangle.Top - penWidth + (rectangle.Height / 2)
        );
        image.Mutate(x => x.DrawText(textOptions, text, Pens.Solid(ColorPalette.Black, penWidth)));

        return this;
    }

    public Image Build()
    {
        return image;
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