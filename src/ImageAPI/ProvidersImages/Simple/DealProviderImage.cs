using ImageAPI.Utils;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.Response;

namespace ImageAPI.ProvidersImages.Simple;

public class DealProviderImage : ProviderImage
{
    public override Image Image { get; }

    public override IDictionary<string, PointF> Coordinates => new Dictionary<string, PointF>
    {
        { "LeftAmount", new PointF(0, 0) }
    };

    public DealProviderImage(ImageProcessor imageProcessor, IEnumerable<Erc721Attribute> attributes)
    {
        foreach (var attribute in attributes)
        {
            var coordinates = GetCoordinates(attribute.TraitType);
            var options = imageProcessor.CreateTextOptions(coordinates);
            imageProcessor.DrawText(attribute, options);
        }

        Image = imageProcessor.Image;
    }
}