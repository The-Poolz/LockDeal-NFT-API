using System.Net;
using SixLabors.ImageSharp;
using ImageAPI.Processing.Drawing;
using MetaDataAPI.Models.DynamoDb;
using Amazon.Lambda.APIGatewayEvents;
using MetaDataAPI.Models.Response;
using SixLabors.ImageSharp.Processing;

namespace ImageAPI.ProvidersImages;

public abstract class ProviderImage
{
    public const string ContentType = "image/png";
    protected readonly DynamoDbItem dynamoDbItem;
    public Image BackgroundImage { get; }

    protected ProviderImage(Image backgroundImage, DynamoDbItem dynamoDbItem)
    {
        BackgroundImage = backgroundImage;
        this.dynamoDbItem = dynamoDbItem;
    }

    public abstract IEnumerable<ToDrawing> ToDrawing();

    public Image DrawOnImage()
    {
        var toDrawing = ToDrawing();
        var image = BackgroundImage.Clone(_ => { });
        return toDrawing.Aggregate(image, (current, drawing) => drawing.Draw(current));
    }

    public static APIGatewayProxyResponse GetResponse(Image image) => GetResponse(Base64FromImage(image));

    public static APIGatewayProxyResponse GetResponse(string base64Image) => new()
    {
        IsBase64Encoded = true,
        StatusCode = (int)HttpStatusCode.OK,
        Body = base64Image,
        Headers = new Dictionary<string, string>
        {
            { "Content-Type", ContentType }
        }
    };

    public static string Base64FromImage(Image image)
    {
        using var outputStream = new MemoryStream();
        image.SaveAsPngAsync(outputStream)
            .GetAwaiter()
            .GetResult();
        var imageBytes = outputStream.ToArray();
        image.SaveAsPng(@"C:\Users\Arden\Desktop\result.png");
        return Convert.ToBase64String(imageBytes);
    }

    protected object GetAttributeValue(string traitType) =>
        dynamoDbItem.Attributes.FirstOrDefault(
            x => x.TraitType == traitType,
            new Erc721Attribute(traitType, $"{traitType} not found.")
        ).Value;
}