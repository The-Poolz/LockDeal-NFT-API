using ImageAPI.Utils;
using Newtonsoft.Json;
using SixLabors.Fonts;
using Amazon.Lambda.Core;
using SixLabors.ImageSharp;
using ImageAPI.ProvidersImages;
using MetaDataAPI.Models.Response;
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

    public APIGatewayProxyResponse Run(APIGatewayProxyRequest input)
    {
        if (!input.QueryStringParameters.ContainsKey("hash"))
        {
            return ResponseBuilder.WrongInput();
        }
        var hash = input.QueryStringParameters["hash"];

        var databaseItem = dynamoDb.GetItemAsync(hash)
            .GetAwaiter()
            .GetResult();
        if (databaseItem.Item.Count == 0)
        {
            return ResponseBuilder.WrongHash();
        }

        var attributes = JsonConvert.DeserializeObject<Erc721Attribute[]>(databaseItem.Item["Data"].S)!;

        try
        {
            var providerImage = ProviderImageFactory.Create(backgroundImage, font, attributes);

            return providerImage.Response;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to process request: {e}");
            return ResponseBuilder.GeneralError();
        }
    }
}
