using SixLabors.Fonts;
using SixLabors.ImageSharp;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.DynamoDb;

namespace ImageAPI.ProvidersImages.Advanced;

public class RefundProviderImage : ProviderImage
{
    public override IDictionary<string, PointF> AttributeCoordinates => new Dictionary<string, PointF>
    {
        //{ "Rate", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 2f) },
        { "MainCoinAmount", new PointF(BackgroundImage.Width - 400, BackgroundImage.Height - 290) },
        //{ "MainCoinCollection", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 4f) }
    };

    public override IDictionary<string, PointF> TextCoordinates => new Dictionary<string, PointF>
    {
        { "MainCoin", new PointF(BackgroundImage.Width - 400, BackgroundImage.Height - 330) }
    };

    public RefundProviderImage(Image backgroundImage, Font font, IList<DynamoDbItem> dynamoDbItems)
        : base(nameof(RefundProvider), backgroundImage, font, dynamoDbItems[0])
    { }
}