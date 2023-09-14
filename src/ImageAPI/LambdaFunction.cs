using ImageAPI.Utils;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ImageAPI;

public class LambdaFunction
{
    private readonly ImageProcessor imageProcessor;

    public LambdaFunction()
        : this(new ImageProcessor())
    { }

    public LambdaFunction(ImageProcessor imageProcessor)
    {
        this.imageProcessor = imageProcessor;
    }

    public async Task<APIGatewayProxyResponse> RunAsync(APIGatewayProxyRequest input)
    {
        if (!int.TryParse(input.QueryStringParameters["id"], out var id))
        {
            return ResponseBuilder.WrongInput();
        }

        try
        {
            var options = imageProcessor.CreateTextOptions(400,100);

            imageProcessor.DrawText(id.ToString(), options);

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
