using SixLabors.Fonts;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.DynamoDb;
using SixLabors.ImageSharp.Processing;

namespace ImageAPI.Utils;

public class ImageFactory
{
    private readonly Font font;
    private Image image;

    public ImageFactory(Image backgroundImage, Font font)
    {
        image = DrawBackgroundImage(backgroundImage);
        this.font = font;
    }

    public ImageFactory DrawProviderName(string providerName)
    {
        var imageProcessor = new ImageProcessor(image, font);
        var options = imageProcessor.CreateTextOptions(new PointF(image.Width - 400, 50));
        imageProcessor.DrawText(providerName, options);
        image = imageProcessor.Image;
        return this;
    }

    public ImageFactory DrawAttributes(DynamoDbItem dynamoDbItem, Func<string, PointF?> getCoordinates)
    {
        var imageProcessor = new ImageProcessor(image, font);

        foreach (var attribute in dynamoDbItem.Attributes)
        {
            var coordinates = getCoordinates(attribute.TraitType);
            if (coordinates == null)
                continue;

            var options = imageProcessor.CreateTextOptions((PointF)coordinates);
            imageProcessor.DrawText(attribute, options);
        }

        image = imageProcessor.Image;
        return this;
    }

    public ImageFactory DrawTexts(IDictionary<string, PointF> coordinatesMapping)
    {
        var imageProcessor = new ImageProcessor(image, font);

        foreach (var mapping in coordinatesMapping)
        {
            var options = imageProcessor.CreateTextOptions(mapping.Value);
            imageProcessor.DrawText(mapping.Key, options);
        }

        image = imageProcessor.Image;
        return this;
    }

    public Image BuildImage() => image;

    private static Image DrawBackgroundImage(Image backgroundImage) => backgroundImage.Clone(_ => { });
}