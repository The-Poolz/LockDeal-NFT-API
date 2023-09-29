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
            "[{\"trait_type\":\"ProviderName\",\"value\":\"BundleProvider\"},{\"trait_type\":\"Token\",\"value\":{\"Name\":\"TokenSynthetic\",\"Symbol\":\"~TokenPoolz\",\"Address\":\"0x43d81a2cf49238484d6960de1df9d430c81cdffc\",\"Decimals\":18}},{\"display_type\":\"number\",\"trait_type\":\"Collection\",\"value\":0},{\"display_type\":\"number\",\"trait_type\":\"LeftAmount\",\"value\":0.000000000000025},{\"trait_type\":\"ProviderName_21\",\"value\":\"DealProvider\"},{\"display_type\":\"number\",\"trait_type\":\"LeftAmount_21\",\"value\":0.0},{\"trait_type\":\"ProviderName_22\",\"value\":\"LockDealProvider\"},{\"display_type\":\"date\",\"trait_type\":\"StartTime_22\",\"value\":1695007012},{\"display_type\":\"number\",\"trait_type\":\"LeftAmount_22\",\"value\":0.000000000000025}]"
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
