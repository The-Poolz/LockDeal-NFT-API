using SixLabors.ImageSharp;
using System.Text.RegularExpressions;
using ImageAPI.Processing.Drawing.Options;
using static ImageAPI.Settings.DrawingSettings;

namespace ImageAPI.Processing.Drawing;

public class DrawProviderName : ToDrawing
{
    private readonly string providerName;

    public DrawProviderName(string providerName)
    {
        this.providerName = providerName;
    }

    public override void Draw(Image drawOn)
    {
        Draw(drawOn, new DrawOptions(Regex.Replace(providerName, "(?<!^)([A-Z])", " $1"), ProviderName.Position, ProviderName.FontSize));
    }
}