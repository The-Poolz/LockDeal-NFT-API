using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using System.Reflection;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ImageAPI;

public class Function
{
    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
    {
        foreach (var resourceName in Assembly.GetExecutingAssembly().GetManifestResourceNames())
        {
            context.Logger.LogLine(resourceName);
        }

        int id;

        if (!int.TryParse(input.QueryStringParameters["id"], out id))
        {
            return ResponseBuilder.WrongInput();
        }

        try
        {
            // Load the image from the embedded resources
            using Stream imageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ImageAPI.Resources.Sample-png-Image-for-Testing.png");
            using Image image = Image.Load(imageStream);

            using Stream fontStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ImageAPI.Resources.LEMONMILK-Regular.otf");
            var fontCollection = new FontCollection();
            var fontFamily = fontCollection.Add(fontStream);
            var font = new Font(fontFamily, 24);

            // Create the options
            TextOptions options = new(font)
            {
                Origin = new PointF(500, 100), // Set the rendering origin.
                TabWidth = 8, // A tab renders as 8 spaces wide
                WrappingLength = 100, // Greater than zero so we will word wrap at 100 pixels wide
                HorizontalAlignment = HorizontalAlignment.Right // Right align
            };

            IBrush brush = Brushes.Solid(Color.Red);
            IPen pen = Pens.Solid(Color.Red, 1);

            // Draws the text with a solid black brush and outline.
            image.Mutate(x => x.DrawText(options, $"I add this id: {id}", brush, pen));

            using var outputStream = new MemoryStream();
            await image.SaveAsPngAsync(outputStream);
            byte[] imageBytes = outputStream.ToArray();

            string base64Image = Convert.ToBase64String(imageBytes);

            return ResponseBuilder.ImageResponse(base64Image);
        }
        catch (Exception e)
        {
            context.Logger.LogLine($"Failed to process request: {e}");
            return ResponseBuilder.GeneralError();
        }
    }
}
