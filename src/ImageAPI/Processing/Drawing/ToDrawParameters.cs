using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace ImageAPI.Processing.Drawing;

public class ToDrawParameters
{
    public string Text { get; set; }
    public PointF Location { get; set; }
    public float FontSize { get; set; }
    public float PenWidth { get; set; }
    public FontStyle FontStyle { get; set; }

    public ToDrawParameters(string text, PointF location, float fontSize, float penWidth = 2f, FontStyle fontStyle = FontStyle.Regular)
    {
        Text = text;
        Location = location;
        FontSize = fontSize;
        PenWidth = penWidth;
        FontStyle = fontStyle;
    }
}