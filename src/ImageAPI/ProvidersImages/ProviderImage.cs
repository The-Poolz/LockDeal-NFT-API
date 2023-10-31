﻿using System.Net;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using Amazon.Lambda.APIGatewayEvents;
using MetaDataAPI.Models.DynamoDb;
using ImageAPI.Utils;
using MetaDataAPI.Providers;

namespace ImageAPI.ProvidersImages;

public abstract class ProviderImage
{
    public Image BackgroundImage { get; }
    public Font Font { get; }
    public Image Image { get; protected set; }
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

    protected ProviderImage(Image backgroundImage, Font font, DynamoDbItem dynamoDbItem)
    {
        BackgroundImage = backgroundImage;
        Font = font;
        Image = Image
            .DrawBackgroundImage(backgroundImage)
            .DrawProviderName(font, nameof(BundleProvider))
            .DrawAttributes(font, dynamoDbItem, GetCoordinates);
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