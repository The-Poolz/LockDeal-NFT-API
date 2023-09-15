using ImageAPI.Utils;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using MetaDataAPI.Models.Response;
using Amazon.Lambda.APIGatewayEvents;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ImageAPI;

public class LambdaFunction
{
    private readonly ImageProcessor imageProcessor;
    private readonly DynamoDb dynamoDb;

    public LambdaFunction()
        : this(new ImageProcessor(), new DynamoDb())
    { }

    public LambdaFunction(ImageProcessor imageProcessor, DynamoDb dynamoDb)
    {
        this.imageProcessor = imageProcessor;
        this.dynamoDb = dynamoDb;
    }

    public async Task<APIGatewayProxyResponse> RunAsync(APIGatewayProxyRequest input)
    {
        if (input.QueryStringParameters.ContainsKey("hash"))
        {
            return ResponseBuilder.WrongInput();
        }
        var hash = input.QueryStringParameters["hash"];

        var databaseItem = await dynamoDb.GetItemAsync(hash);
        if (databaseItem.Item.Count == 0)
        {
            return ResponseBuilder.WrongHash();
        }

        var attributes = JsonConvert.DeserializeObject<IEnumerable<Erc721Attribute>>(databaseItem.Item["Data"].S)!;

        foreach (var attribute in attributes)
        {
            Console.WriteLine(JsonConvert.SerializeObject(attribute));
        }

        try
        {
            var options = imageProcessor.CreateTextOptions(400,100);

            imageProcessor.DrawText(hash, options);

            var base64Image = await imageProcessor.GetBase64ImageAsync();

            return ResponseBuilder.ImageResponse(base64Image);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to process request: {e}");
            return ResponseBuilder.GeneralError();
        }
    }
}
