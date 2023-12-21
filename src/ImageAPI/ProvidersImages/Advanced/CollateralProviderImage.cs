using MetaDataAPI.Models.DynamoDb;
using ImageAPI.Processing.Drawing;

namespace ImageAPI.ProvidersImages.Advanced;

public class CollateralProviderImage : ProviderImage
{
    public CollateralProviderImage(IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(dynamoDbItems[0])
    { }

    protected override IEnumerable<ToDrawing> ToDrawing()
    {
        return new ToDrawing[]
        {
            new DrawLeftAmount(GetAttributeValue("LeftAmount")),
        };
    }
}