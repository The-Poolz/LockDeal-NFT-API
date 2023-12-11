using System.Net;
using ImageAPI.Utils;
using SixLabors.Fonts;
using Amazon.Lambda.Core;
using ImageAPI.Extensions;
using SixLabors.ImageSharp;
using ImageAPI.ProvidersImages;
using Amazon.Lambda.APIGatewayEvents;
using MetaDataAPI.Models.DynamoDb;
using Newtonsoft.Json;
using ImageAPI.Processing;

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

        ImageInfo.Initialize(backgroundImage);
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

        //if (databaseItem.Item.TryGetValue("Image", out var attributeValue))
        //{
        //    return new APIGatewayProxyResponse
        //    {
        //        IsBase64Encoded = true,
        //        StatusCode = (int)HttpStatusCode.OK,
        //        Body = attributeValue.S,
        //        Headers = new Dictionary<string, string>
        //        {
        //            { "Content-Type",  ProviderImage.ContentType }
        //        }
        //    };
        //}

        try
        {
            var attributes = JsonConvert.DeserializeObject<DynamoDbItem[]>("[{\"ProviderName\":\"RefundProvider\",\"Attributes\":[{\"trait_type\":\"Rate\",\"value\":0.000000000000000000167},{\"trait_type\":\"MainCoinAmount\",\"value\":0.000000000000000000000},{\"trait_type\":\"MainCoinCollection\",\"value\":1}]},{\"ProviderName\":\"TimedDealProvider\",\"Attributes\":[{\"trait_type\":\"StartAmount\",\"value\":10000.0},{\"display_type\":\"date\",\"trait_type\":\"FinishTime\",\"value\":1700000196},{\"display_type\":\"date\",\"trait_type\":\"StartTime\",\"value\":1699357690},{\"trait_type\":\"Collection\",\"value\":0},{\"trait_type\":\"LeftAmount\",\"value\":0.0}]}]")!;
            var providerImage = ProviderImageFactory.Create(backgroundImage, font, attributes);

            //await dynamoDb.UpdateItemAsync(hash, providerImage.Base64Image);

            return providerImage.Response;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to process request: {e}");
            return ResponseBuilder.GeneralError();
        }
    }
}
