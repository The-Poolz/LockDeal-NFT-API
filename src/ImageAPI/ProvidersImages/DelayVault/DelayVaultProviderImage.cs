using SixLabors.ImageSharp;
using ImageAPI.Processing.Drawing;
using MetaDataAPI.Models.DynamoDb;

namespace ImageAPI.ProvidersImages.DelayVault;

public class DelayVaultProviderImage : ProviderImage
{
    public DelayVaultProviderImage(Image backgroundImage, IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(backgroundImage, dynamoDbItems[0])
    { }

    protected override IEnumerable<ToDrawing> ToDrawing()
    {
        yield return new DrawLeftAmount(GetAttributeValue("LeftAmount"));
    }
}