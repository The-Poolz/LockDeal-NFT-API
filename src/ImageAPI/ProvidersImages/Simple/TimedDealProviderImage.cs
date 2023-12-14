using SixLabors.ImageSharp;
using MetaDataAPI.Models.DynamoDb;
using ImageAPI.Processing.Drawing;

namespace ImageAPI.ProvidersImages.Simple;

public class TimedDealProviderImage : LockDealProviderImage
{
    public TimedDealProviderImage(Image backgroundImage, IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(backgroundImage, dynamoDbItems)
    { }

    protected override IEnumerable<ToDrawing> ToDrawing()
    {
        foreach (var toDrawing in base.ToDrawing())
        {
            yield return toDrawing;
        }
        yield return new DrawFinishTime(GetAttributeValue("FinishTime"));
    }
}