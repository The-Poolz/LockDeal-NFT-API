using ImageAPI.Utils;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.Response;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageAPI.ProvidersImages.Advanced;

public class BundleProviderImage : ProviderImage
{
    public sealed override Image Image { get; }
    public override IDictionary<string, PointF> Coordinates => new Dictionary<string, PointF>
    {
        { "ProviderName", new PointF(0, 0) },
        { "LeftAmount", new PointF(0, BackgroundImage.Height / 2f) },
        { "StartTime", new PointF(0, BackgroundImage.Height / 3f) },
        { "FinishTime", new PointF(0, BackgroundImage.Height / 4f) },
        { "Collection", new PointF(0, BackgroundImage.Height / 5f) }
    };

    public BundleProviderImage(ImageProcessor imageProcessor, IEnumerable<Erc721Attribute> attributes)
        : base(imageProcessor)
    {
        attributes = attributes.ToArray();
        var groupedAttributes = attributes
            .Where(attr => Regex.IsMatch(attr.TraitType, @"_(\d+)"))
            .GroupBy(attr => Regex.Match(attr.TraitType, @"_(\d+)").Groups[1].Value)
            .Select(group => group.Select(attr =>
            {
                var newTraitType = Regex.Replace(attr.TraitType, @"_\d+$", string.Empty);
                return new Erc721Attribute(
                    newTraitType,
                    attr.Value,
                    attr.DisplayType
                );
            }).ToArray())
            .ToArray();


        var bundleImages = groupedAttributes
            .Select(includedAttributes => ProviderImageFactory.Create(imageProcessor, includedAttributes))
            .Select(providerImage => providerImage.Image)
            .ToArray();

        foreach (var attribute in attributes)
        {
            var coordinates = GetCoordinates(attribute.TraitType);
            if (coordinates == null)
                continue;

            var options = imageProcessor.CreateTextOptions((PointF)coordinates);
            imageProcessor.DrawText(attribute, options);
        }

        Image = imageProcessor.Image;

        var imageList = new List<Image>(bundleImages)
        {
            
        };

        const int frameDelay = 100;

        // Create empty image.
        using Image<Rgba32> gif = new(750, 375, Color.Blue);

        // Set animation loop repeat count to 5.
        var gifMetaData = gif.Metadata.GetGifMetadata();
        gifMetaData.RepeatCount = 5;

        // Set the delay until the next image is displayed.
        var metadata = gif.Frames.RootFrame.Metadata.GetGifMetadata();
        metadata.FrameDelay = frameDelay;
        for (var i = 0; i < imageList.Count; i++)
        {
            // Set the delay until the next image is displayed.
            metadata = imageList[i].Frames.RootFrame.Metadata.GetGifMetadata();
            metadata.FrameDelay = frameDelay;

            // Add the color image to the gif.
            gif.Frames.AddFrame(imageList[i].Frames.RootFrame);
        }
        gif.SaveAsGif("animated.gif");
    }
}