using System.Net;
using Amazon.DynamoDBv2.Model;
using ImageAPI.Utils;
using Newtonsoft.Json;
using SixLabors.Fonts;
using Amazon.Lambda.Core;
using SixLabors.ImageSharp;
using ImageAPI.ProvidersImages;
using MetaDataAPI.Models.DynamoDb;
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
        //if (!input.QueryStringParameters.ContainsKey("hash"))
        //{
        //    return ResponseBuilder.WrongInput();
        //}
        //var hash = input.QueryStringParameters["hash"];

        //var databaseItem = await dynamoDb.GetItemAsync(hash);

        //if (databaseItem.Item.Count == 0)
        //{
        //    return ResponseBuilder.WrongHash();
        //}

        //if (databaseItem.Item.TryGetValue("Images", out var attributeValue))
        //{
        //    if (attributeValue.SS.Count > 1)
        //    {
        //        var providerImage = ProviderImageFactory.Create(backgroundImage, font, ParseAttributes(databaseItem));
        //        return providerImage.Response;
        //    }

        //    return new APIGatewayProxyResponse
        //    {
        //        IsBase64Encoded = true,
        //        StatusCode = (int)HttpStatusCode.OK,
        //        Body = attributeValue.SS[0],
        //        Headers = new Dictionary<string, string>
        //        {
        //            { "Content-Type",  databaseItem.Item["Content-Type"].S }
        //        }
        //    };
        //}

        try
        {
            var providerImage = ProviderImageFactory.Create(backgroundImage, font, ParseAttributes());

            //await dynamoDb.UpdateItemAsync(hash, providerImage.Base64Image, providerImage.ContentType);

            return providerImage.Response;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to process request: {e}");
            return ResponseBuilder.GeneralError();
        }
    }

    private DynamoDbItem[] ParseAttributes() =>
        JsonConvert.DeserializeObject<DynamoDbItem[]>("[{\"ProviderName\":\"RefundProvider\",\"Attributes\":[{\"trait_type\":\"Rate\",\"value\":0.000000000000000142},{\"trait_type\":\"MainCoinAmount\",\"value\":0.000000000000000000},{\"trait_type\":\"MainCoinCollection\",\"value\":1}]},{\"ProviderName\":\"TimedDealProvider\",\"Attributes\":[{\"trait_type\":\"StartAmount\",\"value\":1000.0},{\"display_type\":\"date\",\"trait_type\":\"FinishTime\",\"value\":1702327983},{\"display_type\":\"date\",\"trait_type\":\"StartTime\",\"value\":1698658046},{\"trait_type\":\"Collection\",\"value\":0},{\"trait_type\":\"LeftAmount\",\"value\":0.0}]}]")!;
}
