using SixLabors.ImageSharp;
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
        return Draw(drawOn, providerName, ProviderName.Position, ProviderName.FontSize);
    }
}