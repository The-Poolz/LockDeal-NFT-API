using System.Net;
using SixLabors.Fonts;
using ImageAPI.Processing;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.DynamoDb;
using Amazon.Lambda.APIGatewayEvents;
using ImageAPI.Processing.Drawing;

namespace ImageAPI.ProvidersImages;

public abstract class ProviderImage
{
    public const string ContentType = "image/png";
    public Image BackgroundImage { get; }
    public Image Image { get; protected set; }
    public string Base64Image { get; }
    public abstract IDictionary<string, PointF> AttributeCoordinates { get; }
    public abstract IDictionary<string, PointF> TextCoordinates { get; }
    public abstract ToDrawing[] ToDrawing { get; }
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
        Image = backgroundImage;

        // TODO: It is not safe to call a virtual member in a constructor
        foreach (var drawing in ToDrawing)
        {
            Image = drawing.Draw(Image);
        }

        Base64Image = Base64FromImage(Image);
    }

    protected PointF? GetCoordinates(string traitType)
    {
        if (AttributeCoordinates.TryGetValue(traitType, out var coordinates))
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
        image.SaveAsPng(@"C:\Users\Arden\Desktop\result.png");
        return Convert.ToBase64String(imageBytes);
    }
}