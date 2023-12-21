using SixLabors.ImageSharp;
using ImageAPI.Processing.Drawing;
using ImageAPI.Settings;
using MetaDataAPI.Models.DynamoDb;
using MetaDataAPI.Models.Response;
using SixLabors.ImageSharp.Processing;
using static ImageAPI.Settings.DrawingSettings;

namespace ImageAPI.ProvidersImages;

public abstract class ProviderImage
{
    public const string ContentType = "image/png";
    protected readonly DynamoDbItem dynamoDbItem;

    protected ProviderImage(DynamoDbItem dynamoDbItem)
    {
        this.dynamoDbItem = dynamoDbItem;
    }

    protected abstract IEnumerable<ToDrawing> ToDrawing();

    public Image DrawOnImage()
    {
        var toDrawing = new List<ToDrawing>(ToDrawing())
        {
            new DrawProviderName(dynamoDbItem.ProviderName),
            //new DrawCurrencySymbol("USD", CurrencySymbol.RefundPosition),
            new DrawCurrency("POOLX", CurrencySymbol.TokenPosition),
            new DrawPoolId(dynamoDbItem.PoolId)
        };
        var image = Resources.BackgroundImage.Clone(_ => { });
        toDrawing.ForEach(x => x.Draw(image));
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