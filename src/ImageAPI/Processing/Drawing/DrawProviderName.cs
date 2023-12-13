using SixLabors.ImageSharp;

namespace ImageAPI.Processing.Drawing;

public class DrawProviderName : ToDrawing
{
    public DrawProviderName(string providerName)
        : base(
            text: providerName,
            coordinates: new PointF(ImageSize.Width - 400, 50),
            fontSize: ProviderNameFontSize
        )
    { }
}