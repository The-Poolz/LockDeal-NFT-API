using SixLabors.Fonts;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.Response;

namespace ImageAPI.ProvidersImages.Simple;

public class TimedDealProviderImage : ProviderImage
{
    public override Image Image { get; }
    public override IDictionary<string, PointF> Coordinates => new Dictionary<string, PointF>
    {
        { "ProviderName", new PointF(BackgroundImage.Width / 2f, 0) },
        { "LeftAmount", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 2f) },
        { "StartTime", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 3f) },
        { "FinishTime", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 4f) },
        { "Collection", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 5f) }
    };

    public TimedDealProviderImage(Image backgroundImage, Font font, IEnumerable<Erc721Attribute> attributes)
        : base(backgroundImage, font)
    {
        Image = DrawAttributes(attributes);
    }
}