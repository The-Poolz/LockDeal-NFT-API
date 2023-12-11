using SixLabors.Fonts;
using SixLabors.ImageSharp;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.DynamoDb;

namespace ImageAPI.ProvidersImages.Advanced;

public class CollateralProviderImage : ProviderImage
{
    public override IDictionary<string, PointF> AttributeCoordinates => new Dictionary<string, PointF>
    {
        { "LeftAmount", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 2f) },
        { "Collection", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 3f) },
    };

    public override IDictionary<string, PointF> TextCoordinates { get; }

    public CollateralProviderImage(Image backgroundImage, Font font, IList<DynamoDbItem> dynamoDbItems)
        : base(nameof(CollateralProvider), backgroundImage, font, dynamoDbItems[0])
    { }
}