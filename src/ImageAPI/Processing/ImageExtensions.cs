using SixLabors.Fonts;
using ImageAPI.Settings;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Processing;
using ImageAPI.Processing.Drawing.Options;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ImageAPI.Processing;

public static class ImageExtensions
{
    public static void DrawText(this Image drawOn, DrawOptions options)
    {
        var textOptions = new RichTextOptions(Resources.Font(options.FontSize, options.FontStyle))
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            Origin = new PointF(options.Location.X - options.PenWidth, options.Location.Y - options.PenWidth)
        };

        drawOn.Mutate(x => x.DrawText(
            textOptions,
            options.Text, 
            Brushes.Solid(options.Color), 
            Pens.Solid(options.Color, options.PenWidth
        )));
    }

    public static void DrawCurrencySymbol(this Image drawOn, DrawCurrencyOptions options)
    {
        const int widthPadding = 20;
        const int heightPadding = 8;

        var textOptions = new RichTextOptions(Resources.Font(options.FontSize))
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        var textSize = TextMeasurer.MeasureAdvance(options.Text, textOptions);

        var rectWidth = textSize.Width + widthPadding;
        var rectHeight = textSize.Height + heightPadding;
        var rectangle = new RectangleF(
            options.Location.X - options.PenWidth,
            options.Location.Y - options.PenWidth,
            rectWidth,
            rectHeight
        );

        var cornerRadius = (textSize.Height + heightPadding) / 2;
        var roundedRectPath = CreateRoundedRectanglePath(rectangle, cornerRadius);

        drawOn.Mutate(x => x.Fill(ColorPalette.White, roundedRectPath));

        textOptions.Origin = new PointF(
            rectangle.Left - options.PenWidth + (rectangle.Width / 2),
            rectangle.Top - options.PenWidth + (rectangle.Height / 2)
        );
        drawOn.Mutate(x => x.DrawText(textOptions, options.Text, Pens.Solid(ColorPalette.Black, options.PenWidth)));
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