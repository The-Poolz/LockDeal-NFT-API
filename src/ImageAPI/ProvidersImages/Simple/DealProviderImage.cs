using SixLabors.ImageSharp;
using MetaDataAPI.Models.DynamoDb;
using ImageAPI.Processing.Drawing;

namespace ImageAPI.ProvidersImages.Simple;

public class DealProviderImage : ProviderImage
{
    public DealProviderImage(Image backgroundImage, IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(backgroundImage, dynamoDbItems[0])
    { }

    protected override IEnumerable<ToDrawing> ToDrawing()
    {
        yield return new DrawLeftAmount(GetAttributeValue("LeftAmount"));
        yield return new DrawText("Left Amount", BackgroundImage.Width - 400, BackgroundImage.Height - 330);
    }
}