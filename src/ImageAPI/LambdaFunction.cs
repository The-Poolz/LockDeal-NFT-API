using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using System.Reflection;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ImageAPI;

public class LambdaFunction
{
    private const string BackgroundResourceName = "ImageAPI.Resources.Sample-png-Image-for-Testing.png";
    private const string FontResourceName = "ImageAPI.Resources.LEMONMILK-Regular.otf";
    private readonly Image image;
    private readonly Font font;

    public LambdaFunction()
    {
        image = LoadImageFromEmbeddedResources();
        font = LoadFontFromEmbeddedResources();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
    {
        if (!int.TryParse(input.QueryStringParameters["id"], out var id))
        {
            return ResponseBuilder.WrongInput();
        }

        // TODO: Receive the metadata by ID.
        try
        {
            var options = CreateTextOptions(400,100);

            DrawText(id.ToString(), options);

            var base64Image = await GetBase64ImageAsync();

            return ResponseBuilder.ImageResponse(base64Image);
        }
        catch (Exception e)
        {
            context.Logger.LogLine($"Failed to process request: {e}");
            return ResponseBuilder.GeneralError();
        }
    }

    private async Task<string> GetBase64ImageAsync()
    {
        using var outputStream = new MemoryStream();
        await image.SaveAsPngAsync(outputStream);
        var imageBytes = outputStream.ToArray();

        return Convert.ToBase64String(imageBytes);
    }

    private void DrawText(string text, TextOptions textOptions, IBrush? brush = null, IPen? pen = null)
    {
        brush ??= Brushes.Solid(Color.Black);
        pen ??= Pens.Solid(Color.DarkRed, 2);

        image.Mutate(x => x.DrawText(textOptions, text, brush, pen));
    }

    private TextOptions CreateTextOptions(float x, float y, float wrappingLength = 100)
    {
        return new TextOptions(font)
        {
            Origin = new PointF(x, y),
            TabWidth = 4,
            WrappingLength = wrappingLength,
            HorizontalAlignment = HorizontalAlignment.Right
        };
    }

    private static Image LoadImageFromEmbeddedResources()
    {
        using var imageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(BackgroundResourceName);

        return imageStream == null
            ? throw new FileNotFoundException($"Could not find the embedded resource '{BackgroundResourceName}'. Make sure the resource exists and its 'Build Action' is set to 'Embedded Resource'.")
            : Image.Load(imageStream);
    }

    private static Font LoadFontFromEmbeddedResources(float size = 24f)
    {
        using var fontStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(FontResourceName) ??
            throw new FileNotFoundException($"Could not find the embedded resource '{FontResourceName}'. Make sure the resource exists and its 'Build Action' is set to 'Embedded Resource'.");

        var fontCollection = new FontCollection();
        var fontFamily = fontCollection.Add(fontStream);
        return new Font(fontFamily, size);
    }
}
