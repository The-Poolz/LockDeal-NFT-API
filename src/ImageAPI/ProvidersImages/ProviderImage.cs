using System.Net;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using Amazon.Lambda.APIGatewayEvents;

namespace ImageAPI.ProvidersImages;

public abstract class ProviderImage
{
    public Image BackgroundImage { get; }
    public Font Font { get; }
    public abstract Image Image { get; }
    public abstract IDictionary<string, PointF> Coordinates { get; }
    public virtual string ContentType => "image/png";
    public string Base64Image
    {
        get
        {
            using var outputStream = new MemoryStream();
            Image.SaveAsPngAsync(outputStream)
                .GetAwaiter()
                .GetResult();
            var imageBytes = outputStream.ToArray();

            return Convert.ToBase64String(imageBytes);
        }
    }
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

    protected ProviderImage(Image backgroundImage, Font font)
    {
        BackgroundImage = backgroundImage;
        Font = font;
    }

    protected PointF? GetCoordinates(string traitType)
    {
        if (Coordinates.TryGetValue(traitType, out var coordinates))
        {
            return coordinates;
        }
        return null;
    }
}