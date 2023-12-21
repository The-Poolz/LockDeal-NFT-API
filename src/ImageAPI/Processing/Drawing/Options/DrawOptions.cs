using SixLabors.Fonts;
using ImageAPI.Settings;
using SixLabors.ImageSharp;

namespace ImageAPI.Processing.Drawing.Options;

public class DrawOptions
{
    public string Text { get; }
    public PointF Location { get; }
    public float FontSize { get; }
    public FontStyle FontStyle { get; }
    public float PenWidth { get; }
    public Color Color { get; }

    public DrawOptions(string text, PointF location, float fontSize, FontStyle fontStyle = FontStyle.Regular, float penWidth = 2f, Color color = default)
    {
        Text = text;
        Location = location;
        FontSize = fontSize;
        FontStyle = fontStyle;
        PenWidth = penWidth;
        Color = color == default ? ColorPalette.White : color;
    }
}