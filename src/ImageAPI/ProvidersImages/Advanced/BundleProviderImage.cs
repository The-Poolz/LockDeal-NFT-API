using SixLabors.Fonts;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.Response;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp.Processing;

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

    public BundleProviderImage(Image backgroundImage, Font font, IEnumerable<Erc721Attribute> attributes)
        : base(backgroundImage, font)
    {
        attributes = attributes.ToArray();
        Image = DrawAttributes(attributes);

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
            .Select(includedAttributes => ProviderImageFactory.Create(backgroundImage, font, includedAttributes))
            .Select(providerImage => providerImage.Image)
            .ToArray();
        var images = new List<Image>(bundleImages);

        const int frameDelay = 100;
        using var gif = Image;

        var gifMetaData = gif.Metadata.GetGifMetadata();
        gifMetaData.RepeatCount = 5;

        var metadata = gif.Frames.RootFrame.Metadata.GetGifMetadata();
        metadata.FrameDelay = frameDelay;

        foreach (var image in images)
        {
            metadata = image.Frames.RootFrame.Metadata.GetGifMetadata();
            metadata.FrameDelay = frameDelay;

            gif.Frames.AddFrame(image.Frames.RootFrame);
        }

        Image = gif.Clone(_ => {});
        gif.SaveAsGif("animated.gif");
    }
}