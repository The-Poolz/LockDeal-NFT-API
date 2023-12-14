using SixLabors.ImageSharp;
using MetaDataAPI.Models.DynamoDb;
using ImageAPI.Processing.Drawing;

namespace ImageAPI.ProvidersImages.Simple;

public class LockDealProviderImage : DealProviderImage
{
    public LockDealProviderImage(Image backgroundImage, IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(backgroundImage, dynamoDbItems)
    { }

    protected override IEnumerable<ToDrawing> ToDrawing()
    {
        foreach (var toDrawing in base.ToDrawing())
        {
            yield return toDrawing;
        }
        yield return new DrawStartTime(GetAttributeValue("StartTime"));
        yield return new DrawText("Start Time", BackgroundImage.Width - 1030, BackgroundImage.Height - 330);
    }
}