using ImageAPI.Processing;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.DynamoDb;

namespace ImageAPI.ProvidersImages.DelayVault;

public class DelayVaultProviderImage : ProviderImage
{
    protected override string ProviderName => "Leader Board";

    public DelayVaultProviderImage(IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(dynamoDbItems[0])
    { }

    protected override IEnumerable<Action<Image>> DrawingActions()
    {
        yield return drawOn => drawOn.DrawLeftAmount(GetAttributeValue("LeftAmount"));
    }
}