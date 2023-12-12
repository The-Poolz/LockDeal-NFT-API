using System.Net;
using ImageAPI.Utils;
using Amazon.Lambda.Core;
using ImageAPI.Extensions;
using ImageAPI.Processing;
using SixLabors.ImageSharp;
using ImageAPI.ProvidersImages;
using Amazon.Lambda.APIGatewayEvents;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ImageAPI;

public class LambdaFunction
{
    private readonly DynamoDb dynamoDb;
    private readonly Image backgroundImage;

    public LambdaFunction() : this(new DynamoDb()) { }

    public LambdaFunction(DynamoDb dynamoDb)
    {
        this.dynamoDb = dynamoDb;
        backgroundImage = new ResourcesLoader().LoadImageFromEmbeddedResources();
        ImageSize.Initialize(backgroundImage);
    }

    public async Task<APIGatewayProxyResponse> RunAsync(APIGatewayProxyRequest input)
    {
        if (!input.QueryStringParameters.ContainsKey("hash"))
        {
            return ResponseBuilder.WrongInput();
        }
        var hash = input.QueryStringParameters["hash"];

        var databaseItem = await dynamoDb.GetItemAsync(hash);

        if (databaseItem.Item.Count == 0)
        {
            return ResponseBuilder.WrongHash();
        }

        if (databaseItem.Item.TryGetValue("Image", out var attributeValue))
        {
            return ResponseBuilder.GetResponse(HttpStatusCode.OK, attributeValue.S, ProviderImage.ContentType);
        }

        try
        {
            var attributes = databaseItem.ParseAttributes();
            var providerImage = ProviderImageFactory.Create(backgroundImage, attributes);
            var image = providerImage.DrawOnImage();

            var base64Image = ProviderImage.Base64FromImage(image);

            await dynamoDb.UpdateItemAsync(hash, base64Image);

            return ResponseBuilder.GetResponse(HttpStatusCode.OK, base64Image, ProviderImage.ContentType);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to process request: {e}");
            return ResponseBuilder.GeneralError();
        }
    }
}
