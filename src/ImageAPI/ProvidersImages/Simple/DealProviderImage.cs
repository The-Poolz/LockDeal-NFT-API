using ImageAPI.Utils;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.Response;

namespace ImageAPI.ProvidersImages.Simple;

public class DealProviderImage : ProviderImage
{
    public override Image Image { get; }

    public override IDictionary<string, PointF> Coordinates => new Dictionary<string, PointF>
    {
        { "ProviderName", new PointF(0, 0) },
        { "LeftAmount", new PointF(0, 32) },
        { "Collection", new PointF(0, 64) }
    };

    public DealProviderImage(ImageProcessor imageProcessor, IEnumerable<Erc721Attribute> attributes)
    {
        foreach (var attribute in attributes)
        {
            var coordinates = GetCoordinates(attribute.TraitType);
            if (coordinates == null)
                continue;
            var options = imageProcessor.CreateTextOptions((PointF)coordinates);
            imageProcessor.DrawText(attribute, options);
        }

        Image = imageProcessor.Image;
    }
}