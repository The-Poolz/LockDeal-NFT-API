using System.Net;
using ImageAPI.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.DynamoDb;
using Amazon.Lambda.APIGatewayEvents;

namespace ImageAPI.ProvidersImages;

public abstract class ProviderImage
{
    public const string ContentType = "image/png";
    public Image BackgroundImage { get; }
    public Image Image { get; protected set; }
    public string Base64Image { get; }
    public abstract IDictionary<string, PointF> Coordinates { get; }
    public APIGatewayProxyResponse Response => new()
    {
        IsBase64Encoded = true,
        StatusCode = (int)HttpStatusCode.OK,
        Body = Base64Image,
        Headers = new Dictionary<string, string>
        {
            { "Content-Type", ContentType }
        }
    };

    protected ProviderImage(string providerName, Image backgroundImage, Font font, DynamoDbItem dynamoDbItem)
    {
        BackgroundImage = backgroundImage;
        Image = new ImageFactory(backgroundImage, font)
            .DrawProviderName(providerName)
            .DrawAttributes(dynamoDbItem, GetCoordinates)
            .BuildImage();
        Base64Image = Base64FromImage(Image);
    }

    protected PointF? GetCoordinates(string traitType)
    {
        if (Coordinates.TryGetValue(traitType, out var coordinates))
        {
            return coordinates;
        }
        return null;
    }

    private static string Base64FromImage(Image image)
    {
        using var outputStream = new MemoryStream();
        image.SaveAsPngAsync(outputStream)
            .GetAwaiter()
            .GetResult();
        var imageBytes = outputStream.ToArray();
        return Convert.ToBase64String(imageBytes);
    }
}