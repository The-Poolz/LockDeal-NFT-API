using ImageAPI.Processing;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.DynamoDb;

namespace ImageAPI.ProvidersImages.DelayVault;

public class DelayVaultProviderImage : ProviderImage
{
    public DelayVaultProviderImage(IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(dynamoDbItems[0])
    { }

    protected override IEnumerable<Func<Image, ImageBuilder>> DrawingActions()
    {
        yield return drawOn => new ImageBuilder(drawOn).DrawLeftAmount(GetAttributeValue("LeftAmount"));
    }
}