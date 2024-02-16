using System.Net;
using ImageAPI.Utils;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using ImageAPI.ProvidersImages;
using MetaDataAPI.Models.DynamoDb;
using Amazon.Lambda.APIGatewayEvents;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ImageAPI;

public class LambdaFunction(DynamoDb dynamoDb)
{
    public LambdaFunction() : this(new DynamoDb()) { }

    public async Task<APIGatewayProxyResponse> RunAsync(APIGatewayProxyRequest input)
    {
        if (!input.QueryStringParameters.TryGetValue("hash", out string? hash))
        {
            return ResponseBuilder.WrongInput();
        }

        var databaseItem = await dynamoDb.GetItemAsync(hash);

        if (databaseItem.Item.Count == 0)
        {
            return ResponseBuilder.WrongHash();
        }

        if (databaseItem.Item.TryGetValue("Image", out var attributeValue))
        {
            return ResponseBuilder.GetResponse(HttpStatusCode.OK, attributeValue.S, ProviderImage.ContentType, true);
        }

        try
        {
            var attributes = JsonConvert.DeserializeObject<DynamoDbItem[]>(databaseItem.Item["Data"].S)!;
            var providerImage = ProviderImageFactory.Create(attributes);
            var image = providerImage.DrawOnImage();

            var base64Image = ProviderImage.Base64FromImage(image);

            await dynamoDb.UpdateItemAsync(hash, base64Image);

            return ResponseBuilder.GetResponse(HttpStatusCode.OK, base64Image, ProviderImage.ContentType, true);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to process request: {e}");
            return ResponseBuilder.GeneralError();
        }
    }
}
