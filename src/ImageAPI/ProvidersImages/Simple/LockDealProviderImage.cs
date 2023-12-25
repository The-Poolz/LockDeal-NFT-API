using ImageAPI.Processing;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.DynamoDb;

namespace ImageAPI.ProvidersImages.Simple;

public class LockDealProviderImage : DealProviderImage
{
    public LockDealProviderImage(IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(dynamoDbItems)
    { }

    protected override IEnumerable<Func<Image, ImageBuilder>> DrawingActions()
    {
        foreach (var action in base.DrawingActions())
        {
            yield return action;
        }
        yield return drawOn => new ImageBuilder(drawOn).DrawStartTime(GetAttributeValue("StartTime"));
    }
}