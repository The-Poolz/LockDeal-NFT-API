using SixLabors.Fonts;
using SixLabors.ImageSharp;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.DynamoDb;

namespace ImageAPI.ProvidersImages.Simple;

public class LockDealProviderImage : ProviderImage
{
    public override IDictionary<string, PointF> AttributeCoordinates => new Dictionary<string, PointF>
    {
        { "LeftAmount", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 2f) },
        { "StartTime", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 3f) },
        { "Collection", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 4f) }
    };

    public override IDictionary<string, PointF> TextCoordinates { get; }

    public LockDealProviderImage(Image backgroundImage, Font font, IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(nameof(LockDealProvider), backgroundImage, font, dynamoDbItems[0])
    { }
}