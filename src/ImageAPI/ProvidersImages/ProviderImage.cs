﻿using System.Net;
using ImageAPI.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.DynamoDb;
using Amazon.Lambda.APIGatewayEvents;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageAPI.ProvidersImages;

public abstract class ProviderImage
{
    public Image BackgroundImage { get; }
    public Font Font { get; }
    public Image Image { get; protected set; }
    public string ContentType { get; init; }
    public abstract IDictionary<string, PointF> Coordinates { get; }
    public string Base64Image
    {
        get
        {
            using var outputStream = new MemoryStream();
            if (ContentType == "image/png")
            {
                Image.SaveAsPngAsync(outputStream)
                    .GetAwaiter()
                    .GetResult();
            }
            else if (ContentType == "image/gif")
            {
                Image.SaveAsGifAsync(outputStream)
                    .GetAwaiter()
                    .GetResult();
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

    protected ProviderImage(string providerName, Image backgroundImage, Font font, DynamoDbItem dynamoDbItem)
    {
        BackgroundImage = backgroundImage;
        Font = font;
        Image = new ImageFactory(backgroundImage, font)
            .DrawProviderName(providerName)
            .DrawAttributes(dynamoDbItem, GetCoordinates)
            .BuildImage();
        ContentType = "image/png";
    }

    protected ProviderImage(string providerName, Image backgroundImage, Font font, IList<DynamoDbItem> dynamoDbItems)
        : this(providerName, backgroundImage, font, dynamoDbItems[0])
    {
        dynamoDbItems.Remove(dynamoDbItems[0]);

        var bundleImages = dynamoDbItems.Select(dynamoDbItem => ProviderImageFactory.Create(backgroundImage, font, new[] { dynamoDbItem }))
            .Select(providerImage => providerImage.Image)
            .ToArray();

        var images = new List<Image>
        {
            Image
        };
        images.AddRange(bundleImages);

        const int frameDelay = 10;
        using var gif = Image.Clone(_ => { });
        var gifMetaData = gif.Metadata.GetGifMetadata();
        gifMetaData.RepeatCount = 0;

        GifCreator.SlideImages(gif, images, frameDelay);

        Image = gif.Clone(_ => { });
        ContentType = "image/gif";
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