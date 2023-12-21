using ImageAPI.Processing.Drawing;
using MetaDataAPI.Models.DynamoDb;

namespace ImageAPI.ProvidersImages.DelayVault;

public class DelayVaultProviderImage : ProviderImage
{
    public DelayVaultProviderImage(IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(dynamoDbItems[0])
    { }

    protected override IEnumerable<ToDrawing> ToDrawing()
    {
        yield return new DrawLeftAmount(GetAttributeValue("LeftAmount"));
    }
}