using SixLabors.Fonts;
using MetaDataAPI.Utils;
using System.Globalization;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ImageAPI.Utils;

public class ImageProcessor
{
    private readonly Font font;
    public readonly Image Image;

    public ImageProcessor(float fontSize = 24f)
    {
        var resourcesLoader = new ResourcesLoader();
        Image = resourcesLoader.LoadImageFromEmbeddedResources();
        font = resourcesLoader.LoadFontFromEmbeddedResources(fontSize);
    }

    public virtual async Task<string> GetBase64ImageAsync()
    {
        using var outputStream = new MemoryStream();
        await Image.SaveAsPngAsync(outputStream);
        var imageBytes = outputStream.ToArray();

        return Convert.ToBase64String(imageBytes);
    }

    public virtual void DrawText(Erc721Attribute attribute, TextOptions textOptions, IBrush? brush = null, IPen? pen = null)
    {
        string text;
        if (attribute.DisplayType == DisplayType.String)
        {
            text = attribute.Value.ToString()!;
        }
        else
        {
            text = attribute.DisplayType == DisplayType.Date ? TimeUtils.FromUnixTimestamp((long)attribute.Value).ToString(CultureInfo.InvariantCulture) : attribute.Value.ToString()!;
        }

        DrawText(text, textOptions, brush, pen);
    }

    public virtual void DrawText(string text, TextOptions textOptions, IBrush? brush = null, IPen? pen = null)
    {
        brush ??= Brushes.Solid(Color.Black);
        pen ??= Pens.Solid(Color.DarkRed, 2);

        Image.Mutate(x => x.DrawText(textOptions, text, brush, pen));

        Image.Save("result.jpg");
    }

    public virtual TextOptions CreateTextOptions(PointF coordinates)
    {
        return new TextOptions(font)
        {
            Origin = coordinates,
            TabWidth = 4,
            HorizontalAlignment = HorizontalAlignment.Left
        };
    }
}