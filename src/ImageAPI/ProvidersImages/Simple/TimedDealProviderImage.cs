using SixLabors.ImageSharp;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.DynamoDb;
using ImageAPI.Processing.Drawing;

namespace ImageAPI.ProvidersImages.Simple;

public class TimedDealProviderImage : ProviderImage
{
    public TimedDealProviderImage(Image backgroundImage, IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(backgroundImage, dynamoDbItems[0])
    { }

    public override IEnumerable<ToDrawing> ToDrawing()
    {
        return new ToDrawing[]
        {
            new DrawProviderName(nameof(TimedDealProvider)),
            new DrawLeftAmount(GetAttributeValue("LeftAmount")),
            new DrawText("Left", BackgroundImage.Width - 400, BackgroundImage.Height - 330),
            new DrawStartTime(GetAttributeValue("StartTime")),
            new DrawText("Start", BackgroundImage.Width - 1030, BackgroundImage.Height - 330),
            new DrawFinishTime(GetAttributeValue("FinishTime")),
            new DrawText("Finish", BackgroundImage.Width - 730, BackgroundImage.Height - 330),
        };
    }
}