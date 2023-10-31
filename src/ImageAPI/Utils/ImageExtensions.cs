using SixLabors.Fonts;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.DynamoDb;
using SixLabors.ImageSharp.Processing;

namespace ImageAPI.Utils;

public static class ImageExtensions
{
    public static Image DrawBackgroundImage(this Image? image, Image backgroundImage)
    {
        return backgroundImage.Clone(_ => { });
    }

    public static Image DrawProviderName(this Image image, Font font, string providerName)
    {
        var imageProcessor = new ImageProcessor(image, font);
        var options = imageProcessor.CreateTextOptions(new PointF(50, 50));
        imageProcessor.DrawText(providerName, options);
        return imageProcessor.Image;
    }

    public static Image DrawAttributes(this Image image, Font font, DynamoDbItem dynamoDbItem, Func<string, PointF?> getCoordinates)
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

        return imageProcessor.Image;
    }
}