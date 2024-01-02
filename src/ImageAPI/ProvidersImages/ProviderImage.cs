using ImageAPI.Settings;
using ImageAPI.Processing;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.DynamoDb;
using MetaDataAPI.Models.Response;
using SixLabors.ImageSharp.Processing;

namespace ImageAPI.ProvidersImages;

public abstract class ProviderImage
{
    public const string ContentType = "image/png";
    protected readonly DynamoDbItem dynamoDbItem;
    protected virtual string ProviderName => dynamoDbItem.ProviderName;

    protected ProviderImage(DynamoDbItem dynamoDbItem)
    {
        this.dynamoDbItem = dynamoDbItem;
    }

    protected abstract IEnumerable<Action<Image>> DrawingActions();

    public Image DrawOnImage()
    {
        var actions = new List<Action<Image>>(DrawingActions())
        {
            drawOn => drawOn.DrawProviderName(ProviderName),
            drawOn => drawOn.DrawPoolId(dynamoDbItem.PoolId),
            drawOn => drawOn.DrawTokenBadge(dynamoDbItem.TokenSymbol)
        };

        var image = Resources.BackgroundImage.Clone(_ => { });
        actions.ForEach(action => action(image));
        return image;
    }

    public static string Base64FromImage(Image image)
    {
#if DEBUG
        image.SaveAsPng("result.png");
#endif
        using var outputStream = new MemoryStream();
        image.SaveAsPngAsync(outputStream)
            .GetAwaiter()
            .GetResult();
        var imageBytes = outputStream.ToArray();
        return Convert.ToBase64String(imageBytes);
    }

    protected object GetAttributeValue(string traitType) =>
        dynamoDbItem.Attributes.FirstOrDefault(
            x => x.TraitType == traitType,
            new Erc721Attribute(traitType, $"{traitType} not found.")
        ).Value;
}