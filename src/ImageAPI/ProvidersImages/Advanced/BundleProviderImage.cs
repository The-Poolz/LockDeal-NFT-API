using ImageAPI.Utils;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.Response;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Processing.Processors.Quantization;
using SixLabors.ImageSharp.Processing;
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
            Image
        };

        const int frameDelay = 100;

        // For demonstration: use images with different colors.
        Color[] colors = {
            Color.Green,
            Color.Red
        };

        // Create empty image.
        using Image<Rgba32> gif = new(width, height, Color.Blue);

        // Set animation loop repeat count to 5.
        var gifMetaData = gif.Metadata.GetGifMetadata();
        gifMetaData.RepeatCount = 5;

        // Set the delay until the next image is displayed.
        GifFrameMetadata metadata = gif.Frames.RootFrame.Metadata.GetGifMetadata();
        metadata.FrameDelay = frameDelay;
        for (int i = 0; i < colors.Length; i++)
        {
            // Create a color image, which will be added to the gif.
            using Image<Rgba32> image = new(width, height, colors[i]);

            // Set the delay until the next image is displayed.
            metadata = image.Frames.RootFrame.Metadata.GetGifMetadata();
            metadata.FrameDelay = frameDelay;

            // Add the color image to the gif.
            gif.Frames.AddFrame(image.Frames.RootFrame);
        }

        // Save the final result.
        gif.SaveAsGif("output.gif");

        // Сохраняем GIF
        gif.SaveAsGif("animated.gif");
        }
    }
}