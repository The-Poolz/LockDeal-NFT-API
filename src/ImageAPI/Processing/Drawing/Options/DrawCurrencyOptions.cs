using SixLabors.ImageSharp;
using static ImageAPI.Settings.DrawingSettings;

namespace ImageAPI.Processing.Drawing.Options;

public class DrawCurrencyOptions
{
    public string Text { get; }
    public PointF Location { get; }
    public float FontSize => Currency.FontSize;
    public float PenWidth => 2f;

    public DrawCurrencyOptions(string currencySymbol, PointF location)
    {
        Text = $"${currencySymbol}";
        Location = location;
    }
}