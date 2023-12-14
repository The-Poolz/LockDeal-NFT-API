using SixLabors.ImageSharp;
using MetaDataAPI.Models.DynamoDb;
using ImageAPI.Processing.Drawing;

namespace ImageAPI.ProvidersImages.Advanced;

public class RefundProviderImage : ProviderImage
{
    public RefundProviderImage(Image backgroundImage, IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(backgroundImage, dynamoDbItems[0])
    { }

    protected override IEnumerable<ToDrawing> ToDrawing()
    {
        return new ToDrawing[]
        {
            new DrawLeftAmount(GetAttributeValue("MainCoinAmount")),
            new DrawText("MainCoin", BackgroundImage.Width - 400, BackgroundImage.Height - 330),
        };
    }
}