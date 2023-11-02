using System.Net;
using ImageAPI.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.DynamoDb;
using Amazon.Lambda.APIGatewayEvents;

namespace ImageAPI.ProvidersImages;

public abstract class ProviderImage
{
    public Image BackgroundImage { get; }
    public Image Image { get; protected set; }
    public Image[] Images { get; protected set; }
    public string ContentType { get; init; }
    public abstract IDictionary<string, PointF> Coordinates { get; }
    public string Base64Image
    {
        get
        {
            using var outputStream = new MemoryStream();
            if (ContentType == ContentTypes.Png)
            {
                Image.SaveAsPngAsync(outputStream)
                    .GetAwaiter()
                    .GetResult();
            }
            else if (ContentType == ContentTypes.Gif)
            {
                Image.SaveAsGifAsync(outputStream)
                    .GetAwaiter()
                    .GetResult();
                Image.SaveAsGif(@"C:\Users\Arden\Desktop\result.gif");
            }
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

    protected ProviderImage(Image backgroundImage, IEnumerable<string> base64Images)
    {
        BackgroundImage = backgroundImage;
        Images = base64Images.Select(x => Image.Load(Convert.FromBase64String(x))).ToArray();

        Image = GifCreator.ImagesToGif(Images);
        ContentType = ContentTypes.Gif;
    }

    protected ProviderImage(string providerName, Image backgroundImage, Font font, DynamoDbItem dynamoDbItem)
    {
        BackgroundImage = backgroundImage;
        Image = new ImageFactory(backgroundImage, font)
            .DrawProviderName(providerName)
            .DrawAttributes(dynamoDbItem, GetCoordinates)
            .BuildImage();
        Images = new[] { Image };
        ContentType = ContentTypes.Png;
    }

    protected ProviderImage(string providerName, Image backgroundImage, Font font, IList<DynamoDbItem> dynamoDbItems)
        : this(providerName, backgroundImage, font, dynamoDbItems[0])
    {
        var bundleImages = dynamoDbItems.Skip(1)
            .Select(dynamoDbItem => ProviderImageFactory.Create(backgroundImage, font, new[] { dynamoDbItem }))
            .Select(providerImage => providerImage.Image)
            .ToArray();

        Images = new List<Image> { Image }.Concat(bundleImages).ToArray();

        Image = GifCreator.ImagesToGif(Images);
        ContentType = ContentTypes.Gif;
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