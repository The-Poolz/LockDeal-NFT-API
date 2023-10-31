using SixLabors.Fonts;
using SixLabors.ImageSharp;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.DynamoDb;

namespace ImageAPI.ProvidersImages.Simple;

public class DealProviderImage : ProviderImage
{
    public override string ProviderName => nameof(DealProvider);
    public override Image Image { get; }
    public override IDictionary<string, PointF> Coordinates => new Dictionary<string, PointF>
    {
        { "LeftAmount", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 2f) },
        { "Collection", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 3f) }
    };

    public DealProviderImage(Image backgroundImage, Font font, IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(backgroundImage, font)
    {
        Image = DrawProviderName();
        Image = DrawAttributes(dynamoDbItems[0]);
    }
}