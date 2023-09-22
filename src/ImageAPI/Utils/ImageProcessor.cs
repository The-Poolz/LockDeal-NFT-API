using System.Globalization;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ImageAPI.Utils;

public class ImageProcessor
{
    private readonly Image image;
    private readonly Font font;

    public ImageProcessor(float fontSize = 24f)
    {
        var resourcesLoader = new ResourcesLoader();
        image = resourcesLoader.LoadImageFromEmbeddedResources();
        font = resourcesLoader.LoadFontFromEmbeddedResources(fontSize);
    }

    public virtual async Task<string> GetBase64ImageAsync()
    {
        using var outputStream = new MemoryStream();
        await image.SaveAsPngAsync(outputStream);
        var imageBytes = outputStream.ToArray();

        return Convert.ToBase64String(imageBytes);
    }

    public virtual void DrawText(Erc721Attribute attribute, TextOptions textOptions, IBrush? brush = null, IPen? pen = null)
    {
        string text;
        if (attribute.DisplayType == null)
        {
            text = attribute.Value.ToString()!;
        }
        else
        {
            var displayType = Enum.Parse<DisplayType>(attribute.DisplayType);

            text = displayType == DisplayType.Date ? TimeUtils.FromUnixTimestamp((long)attribute.Value).ToString(CultureInfo.InvariantCulture) : attribute.Value.ToString()!;
        }

        DrawText(text, textOptions, brush, pen);
    }

    public virtual void DrawText(string text, TextOptions textOptions, IBrush? brush = null, IPen? pen = null)
    {
        brush ??= Brushes.Solid(Color.Black);
        pen ??= Pens.Solid(Color.DarkRed, 2);

        image.Mutate(x => x.DrawText(textOptions, text, brush, pen));
    }

    public virtual TextOptions CreateTextOptions(float x, float y, float wrappingLength = 100)
    {
        return new TextOptions(font)
        {
            Origin = new PointF(x, y),
            TabWidth = 4,
            WrappingLength = wrappingLength,
            HorizontalAlignment = HorizontalAlignment.Left
        };
    }
}