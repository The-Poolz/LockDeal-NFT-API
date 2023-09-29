using SixLabors.Fonts;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.Response;

namespace ImageAPI.ProvidersImages.Simple;

public class DealProviderImage : ProviderImage
{
    public override Image Image { get; }
    public override IDictionary<string, PointF> Coordinates => new Dictionary<string, PointF>
    {
        { "ProviderName", new PointF(BackgroundImage.Width / 2f, 0) },
        { "LeftAmount", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 2f) },
        { "Collection", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 3f) }
    };

    public DealProviderImage(Image backgroundImage, Font font, IEnumerable<Erc721Attribute> attributes)
        : base(backgroundImage, font)
    {
        Image = DrawAttributes(attributes);
    }
}