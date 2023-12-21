using MetaDataAPI.Models.DynamoDb;
using ImageAPI.Processing.Drawing;

namespace ImageAPI.ProvidersImages.Simple;

public class LockDealProviderImage : DealProviderImage
{
    public LockDealProviderImage(IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(dynamoDbItems)
    { }

    protected override IEnumerable<ToDrawing> ToDrawing()
    {
        foreach (var toDrawing in base.ToDrawing())
        {
            yield return toDrawing;
        }
        yield return new DrawStartTime(GetAttributeValue("StartTime"));
    }
}