using ImageAPI.Settings;
using MetaDataAPI.Models.DynamoDb;
using ImageAPI.Processing.Drawing;

namespace ImageAPI.ProvidersImages.Advanced;

public class RefundProviderImage : ProviderImage
{
    public RefundProviderImage(IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(dynamoDbItems[0])
    { }

    protected override IEnumerable<ToDrawing> ToDrawing()
    {
        return new ToDrawing[]
        {
            new DrawLeftAmount(GetAttributeValue("MainCoinAmount")),
            new DrawText("MainCoin", Resources.BackgroundImage.Width - 400, Resources.BackgroundImage.Height - 330),
        };
    }
}