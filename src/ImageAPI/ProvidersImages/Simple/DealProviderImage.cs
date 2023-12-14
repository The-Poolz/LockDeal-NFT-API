using SixLabors.ImageSharp;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.DynamoDb;
using ImageAPI.Processing.Drawing;

namespace ImageAPI.ProvidersImages.Simple;

public class DealProviderImage : ProviderImage
{
    public DealProviderImage(Image backgroundImage, IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(backgroundImage, dynamoDbItems[0])
    { }

    public override IEnumerable<ToDrawing> ToDrawing()
    {
        return new ToDrawing[]
        {
            new DrawProviderName(nameof(DealProvider)),
            new DrawLeftAmount(GetAttributeValue("LeftAmount")),
            new DrawText("Left Amount", BackgroundImage.Width - 400, BackgroundImage.Height - 330),
        };
    }
}