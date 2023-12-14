using SixLabors.ImageSharp;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.DynamoDb;
using ImageAPI.Processing.Drawing;

namespace ImageAPI.ProvidersImages.Simple;

public class LockDealProviderImage : ProviderImage
{
    public LockDealProviderImage(Image backgroundImage, IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(backgroundImage, dynamoDbItems[0])
    { }

    public override IEnumerable<ToDrawing> ToDrawing()
    {
        return new ToDrawing[]
        {
            new DrawProviderName(nameof(LockDealProvider)),
            new DrawLeftAmount(GetAttributeValue("LeftAmount")),
            new DrawText("Left Amount", BackgroundImage.Width - 400, BackgroundImage.Height - 330),
            new DrawStartTime(GetAttributeValue("StartTime")),
            new DrawText("Start Time", BackgroundImage.Width - 1030, BackgroundImage.Height - 330),
        };
    }
}