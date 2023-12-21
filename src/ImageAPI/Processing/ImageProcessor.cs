using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ImageAPI.Processing;

public class ImageProcessor
{
    private readonly Font font;
    private readonly Image image;
    private readonly Color white;
    private readonly Color black;

    public ImageProcessor(Image image, Font font)
    {
        this.image = image;
        this.font = font;
        white = Color.ParseHex("#FDFDFD");
        black = Color.ParseHex("#010013");
    }

    public virtual Image DrawText(string text, PointF location, float penWidth, Color color = default)
    {
        color = color == default ? white : color;
        var textOptions = new RichTextOptions(font)
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            Origin = new PointF(location.X - penWidth, location.Y - penWidth),
        };

        image.Mutate(x => x.DrawText(textOptions, text, Brushes.Solid(color), Pens.Solid(color, penWidth)));

        return image;
    }

    public virtual Image DrawCurrencySymbol(string currencySymbol, PointF coordinates)
    {
        const int widthPadding = 20;
        const int heightPadding = 8;
        const int penWidth = 2;

        var textOptions = new RichTextOptions(font)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        var textSize = TextMeasurer.MeasureAdvance(currencySymbol, textOptions);

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

        image.Mutate(x => x.Fill(white, roundedRectPath));

        textOptions.Origin = new PointF(
            rectangle.Left - penWidth + (rectangle.Width / 2),
            rectangle.Top - penWidth + (rectangle.Height / 2)
        );
        image.Mutate(x => x.DrawText(textOptions, currencySymbol, Pens.Solid(black, penWidth)));

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