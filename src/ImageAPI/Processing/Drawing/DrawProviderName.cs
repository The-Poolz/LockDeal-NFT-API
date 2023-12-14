using SixLabors.ImageSharp;
using System.Text.RegularExpressions;
using static ImageAPI.Processing.DrawingSettings;

namespace ImageAPI.Processing.Drawing;

public class DrawProviderName : ToDrawing
{
    private readonly string providerName;

    public DrawProviderName(string providerName)
    {
        this.providerName = providerName;
    }

    public override Image Draw(Image drawOn)
    {
        return Draw(drawOn, Regex.Replace(providerName, "(?<!^)([A-Z])", " $1"), ProviderName.Position, ProviderName.FontSize);
    }
}