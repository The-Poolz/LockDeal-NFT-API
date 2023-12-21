using SixLabors.ImageSharp;

namespace ImageAPI.Processing.Drawing.Options;

public class DrawCurrencyOptions
{
    public string Text { get; }
    public PointF Location { get; }
    public float FontSize => 16f;
    public float PenWidth => 2f;

    public DrawCurrencyOptions(string currencySymbol, PointF location)
    {
        Text = $"${currencySymbol}";
        Location = location;
    }
}