using ImageAPI.Utils;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using MetaDataAPI.Models.Response;
using Amazon.Lambda.APIGatewayEvents;
using ImageAPI.ProvidersImages;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ImageAPI;

public class LambdaFunction
{
    private const float fontSize = 14f;
    private readonly ImageProcessor imageProcessor;
    private readonly DynamoDb dynamoDb;

    public LambdaFunction()
        : this(new ImageProcessor(fontSize), new DynamoDb())
    { }

    public LambdaFunction(ImageProcessor imageProcessor, DynamoDb dynamoDb)
    {
        this.imageProcessor = imageProcessor;
        this.dynamoDb = dynamoDb;
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
        var attributes = JsonConvert.DeserializeObject<Erc721Attribute[]>("[{\"display_type\":\"number\",\"trait_type\":\"LeftAmount\",\"value\":50.0},{\"display_type\":\"date\",\"trait_type\":\"FinishTime\",\"value\":1696005796},{\"trait_type\":\"MainCoin\",\"value\":\"0xcd1ef832eb8a5a77842c440032e03c4330974d21\"},{\"trait_type\":\"Token\",\"value\":\"0x43d81a2cf49238484d6960de1df9d430c81cdffc\"},{\"trait_type\":\"ProviderName\",\"value\":\"CollateralProvider\"},{\"trait_type\":\"TokenName\",\"value\":\"MaincoinSynthetic\"}]")!;

        try
        {
            var providerImage = ProviderImageFactory.Create(imageProcessor, attributes);

            return ResponseBuilder.ImageResponse(providerImage.Base64Image);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to process request: {e}");
            return ResponseBuilder.GeneralError();
        }
    }
}
