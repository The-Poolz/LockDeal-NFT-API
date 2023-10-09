using ImageAPI.Utils;
using Newtonsoft.Json;
using SixLabors.Fonts;
using Amazon.Lambda.Core;
using SixLabors.ImageSharp;
using ImageAPI.ProvidersImages;
using MetaDataAPI.Models.Response;
using Amazon.Lambda.APIGatewayEvents;
using System.Net;

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

        if (databaseItem.Item.TryGetValue("Image", out var base64Image))
        {
            return new APIGatewayProxyResponse
            {
                IsBase64Encoded = true,
                StatusCode = (int)HttpStatusCode.OK,
                Body = base64Image.S,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type",  databaseItem.Item["Content-Type"].S}
                }
            };
        }

        var attributes = JsonConvert.DeserializeObject<Erc721Attribute[]>(databaseItem.Item["Data"].S)!;

        try
        {
            var providerImage = ProviderImageFactory.Create(backgroundImage, font, attributes);

            await dynamoDb.UpdateItemAsync(hash, providerImage.Base64Image, providerImage.ContentType);

            return providerImage.Response;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to process request: {e}");
            return ResponseBuilder.GeneralError();
        }
    }
}
