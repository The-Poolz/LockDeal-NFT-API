using SixLabors.Fonts;
using ImageAPI.Settings;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ImageAPI.Processing;

public static class BadgeDrawer
{
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