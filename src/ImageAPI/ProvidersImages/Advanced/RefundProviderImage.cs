using SixLabors.Fonts;
using SixLabors.ImageSharp;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.DynamoDb;

namespace ImageAPI.ProvidersImages.Advanced;

public class RefundProviderImage : ProviderImage
{
    public override IDictionary<string, PointF> Coordinates => new Dictionary<string, PointF>
    {
        { "Rate", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 2f) },
        { "MainCoinAmount", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 3f) },
        { "MainCoinCollection", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 4f) }
    };

    public RefundProviderImage(Image backgroundImage, Font font, IList<DynamoDbItem> dynamoDbItems)
        : base(nameof(RefundProvider), backgroundImage, font, dynamoDbItems)
    { }
}