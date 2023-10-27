using SixLabors.Fonts;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.DynamoDb;

namespace ImageAPI.ProvidersImages.Advanced;

public class CollateralProviderImage : ProviderImage
{
    public override Image Image { get; }
    public override IDictionary<string, PointF> Coordinates => new Dictionary<string, PointF>
    {
        { "ProviderName", new PointF(BackgroundImage.Width / 2f, 0) },
        { "MainCoinCollection", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 2f) },
        { "MainCoinCollectorAmount", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 3f) },
        { "TokenCollectorAmount", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 4f) },
        { "MainCoinHolderAmount", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 5f) },
        { "FinishTimestamp", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 6f) },
        { "Collection", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 7f) },
    };

    public CollateralProviderImage(Image backgroundImage, Font font, IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(backgroundImage, font)
    {
        Image = DrawAttributes(dynamoDbItems[0]);
    }
}