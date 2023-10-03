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

    public LambdaFunction()
        : this(new DynamoDb())
    { }

    public LambdaFunction(DynamoDb dynamoDb)
    {
        this.dynamoDb = dynamoDb;
        var resourcesLoader = new ResourcesLoader();
        backgroundImage = resourcesLoader.LoadImageFromEmbeddedResources();
        font = resourcesLoader.LoadFontFromEmbeddedResources(fontSize);
    }

    public APIGatewayProxyResponse Run(APIGatewayProxyRequest input)
    {
        //if (input.QueryStringParameters.ContainsKey("hash"))
        //{
        //    return ResponseBuilder.WrongInput();
        //}
        //var hash = input.QueryStringParameters["hash"];

        //var databaseItem = await dynamoDb.GetItemAsync(hash);
        //if (databaseItem.Item.Count == 0)
        //{
        //    return ResponseBuilder.WrongHash();
        //}

        //var attributes = JsonConvert.DeserializeObject<Erc721Attribute[]>(databaseItem.Item["Data"].S)!;
        var attributes = JsonConvert.DeserializeObject<Erc721Attribute[]>(
            "[{\"trait_type\":\"ProviderName\",\"value\":\"RefundProvider\"},{\"trait_type\":\"Rate\",\"value\":50.0},{\"trait_type\":\"MainCoinAmount\",\"value\":2500.0},{\"trait_type\":\"MainCoinCollection\",\"value\":1},{\"trait_type\":\"SubProviderName\",\"value\":\"DealProvider\"},{\"trait_type\":\"Collection\",\"value\":0},{\"trait_type\":\"LeftAmount\",\"value\":50.0}]"
            )!;

        try
        {
            var providerImage = ProviderImageFactory.Create(backgroundImage, font, attributes);

            return ResponseBuilder.ImageResponse(providerImage.Base64Image);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to process request: {e}");
            return ResponseBuilder.GeneralError();
        }
    }
}
