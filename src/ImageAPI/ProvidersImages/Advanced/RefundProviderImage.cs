using ImageAPI.Settings;
using ImageAPI.Processing;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.DynamoDb;

namespace ImageAPI.ProvidersImages.Advanced;

public class RefundProviderImage : ProviderImage
{
    public RefundProviderImage(IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(dynamoDbItems[0])
    { }

    protected override IEnumerable<Func<Image, ImageBuilder>> DrawingActions()
    {
        yield return drawOn => new ImageBuilder(drawOn)
            .DrawLeftAmount(GetAttributeValue("MainCoinAmount"))
            .DrawText("MainCoin", new PointF(Resources.BackgroundImage.Width - 400, Resources.BackgroundImage.Height - 330), 16f);
    }
}