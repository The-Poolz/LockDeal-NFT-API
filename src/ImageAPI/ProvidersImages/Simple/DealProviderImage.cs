using ImageAPI.Processing;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.DynamoDb;

namespace ImageAPI.ProvidersImages.Simple;

public class DealProviderImage : ProviderImage
{
    public DealProviderImage(IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(dynamoDbItems[0])
    { }

    protected override IEnumerable<Action<Image>> DrawingActions()
    {
        yield return drawOn => drawOn.DrawLeftAmount(GetAttributeValue("LeftAmount"));
    }
}