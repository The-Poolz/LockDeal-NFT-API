using System.Net;
using ImageAPI.Utils;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using SixLabors.ImageSharp;
using ImageAPI.ProvidersImages;
using MetaDataAPI.Models.DynamoDb;
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
        //    return ResponseBuilder.GetResponse(HttpStatusCode.OK, attributeValue.S, ProviderImage.ContentType, true);
        //}

        try
        {
            var attributes = JsonConvert.DeserializeObject<DynamoDbItem[]>("[{\"ProviderName\":\"TimedDealProvider\",\"Attributes\":[{\"trait_type\":\"StartAmount\",\"value\":1065.12406},{\"display_type\":\"date\",\"trait_type\":\"FinishTime\",\"value\":1699545629},{\"display_type\":\"date\",\"trait_type\":\"StartTime\",\"value\":1699459229},{\"trait_type\":\"Collection\",\"value\":2},{\"trait_type\":\"LeftAmount\",\"value\":1065.12406}]}]")!;
            //var attributes = JsonConvert.DeserializeObject<DynamoDbItem[]>(databaseItem.Item["Data"].S)!;
            var providerImage = ProviderImageFactory.Create(backgroundImage, attributes);
            var image = providerImage.DrawOnImage();

            var base64Image = ProviderImage.Base64FromImage(image);

            //await dynamoDb.UpdateItemAsync(hash, base64Image);

            return ResponseBuilder.GetResponse(HttpStatusCode.OK, base64Image, ProviderImage.ContentType, true);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to process request: {e}");
            return ResponseBuilder.GeneralError();
        }
    }
}
