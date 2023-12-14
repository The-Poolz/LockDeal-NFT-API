using SixLabors.ImageSharp;

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
        return Draw(drawOn, providerName, new PointF(ImageSize.Width - 400, 50), ProviderNameFontSize);
    }
}