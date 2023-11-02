using System.Net;
using ImageAPI.Utils;
using SixLabors.Fonts;
using Amazon.Lambda.Core;
using ImageAPI.Extensions;
using SixLabors.ImageSharp;
using ImageAPI.ProvidersImages;
using Amazon.Lambda.APIGatewayEvents;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ImageAPI;

public class LambdaFunction
{
    private const float fontSize = 14f;
    private readonly DynamoDb dynamoDb;
    private readonly Font font;
    private readonly Image backgroundImage;

    public LambdaFunction() : this(new DynamoDb()) { }

    public LambdaFunction(DynamoDb dynamoDb)
    {
        this.dynamoDb = dynamoDb;
        var resourcesLoader = new ResourcesLoader();
        backgroundImage = resourcesLoader.LoadImageFromEmbeddedResources();
        font = resourcesLoader.LoadFontFromEmbeddedResources(fontSize);
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

        if (databaseItem.Item.TryGetValue("Images", out var attributeValue))
        {
            if (attributeValue.SS.Count > 1)
            {
                var providerImage = ProviderImageFactory.Create(backgroundImage, font, databaseItem.ParseAttributes());
                return providerImage.Response;
            }

            return new APIGatewayProxyResponse
            {
                IsBase64Encoded = true,
                StatusCode = (int)HttpStatusCode.OK,
                Body = attributeValue.SS[0],
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type",  databaseItem.Item["Content-Type"].S }
                }
            };
        }

        try
        {
            var providerImage = ProviderImageFactory.Create(backgroundImage, font, databaseItem.ParseAttributes());

            await dynamoDb.UpdateItemAsync(hash, providerImage.Base64Images, providerImage.ContentType);

            return providerImage.Response;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to process request: {e}");
            return ResponseBuilder.GeneralError();
        }
    }
}
