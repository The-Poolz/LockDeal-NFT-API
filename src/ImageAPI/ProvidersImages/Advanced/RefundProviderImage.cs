using SixLabors.Fonts;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.DynamoDb;

namespace ImageAPI.ProvidersImages.Advanced;

public class RefundProviderImage : ProviderImage
{
    public override IDictionary<string, PointF> Coordinates => new Dictionary<string, PointF>
    {
        { "Rate", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 2f) },
        { "MainCoinAmount", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 3f) },
        { "MainCoinCollection", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 4f) },
        { "SubProviderName", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 5f) },
        { "Collection", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 6f) },
        { "LeftAmount", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 7f) },
    };

    public RefundProviderImage(Image backgroundImage, Font font, IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(backgroundImage, font, dynamoDbItems[0])
    { }
}