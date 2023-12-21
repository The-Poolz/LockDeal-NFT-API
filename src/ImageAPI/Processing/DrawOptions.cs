using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace ImageAPI.Processing;

public class DrawOptions
{
    public string Text { get; }
    public PointF Location { get; }
    public float FontSize { get; }
    public FontStyle FontStyle { get; }
    public float PenWidth { get; }

    public DrawOptions(string text, PointF location, float fontSize, FontStyle fontStyle = FontStyle.Regular, float penWidth = 2f, Color color = default)
    {
        Text = text;
        Location = location;
        FontSize = fontSize;
        FontStyle = fontStyle;
        PenWidth = penWidth;
    }
}