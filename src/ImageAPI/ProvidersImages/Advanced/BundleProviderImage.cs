using SixLabors.Fonts;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.Response;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageAPI.ProvidersImages.Advanced;

public class BundleProviderImage : ProviderImage
{
    public sealed override Image Image { get; }
    public override IDictionary<string, PointF> Coordinates => new Dictionary<string, PointF>
    {
        { "ProviderName", new PointF(BackgroundImage.Width / 2f, 0) },
        { "LeftAmount", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 2f) },
        { "StartTime", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 3f) },
        { "FinishTime", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 4f) },
        { "Collection", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 5f) }
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
        var images = new List<Image>
        {
            Image
        };
        images.AddRange(bundleImages);

        const int frameDelay = 10;
        using var gif = Image.Clone(_ => {});
        var gifMetaData = gif.Metadata.GetGifMetadata();
        gifMetaData.RepeatCount = 0;

        SlideImages(gif, images, frameDelay);

        Image = gif.Clone(_ => {});
        gif.SaveAsGif("animated.gif");
    }

    private static void SlideImages(Image gif, IReadOnlyList<Image> images, int frameDelay)
    {
        const int slideSteps = 20;
        var stepSize = images[0].Width / slideSteps;

        const int staticFrameDelay = 500;

        for (var i = 0; i < images.Count; i++)
        {
            var currentImage = images[i];
            var nextImage = images[(i + 1) % images.Count];

            using (var staticFrame = new Image<Rgba32>(currentImage.Width, currentImage.Height))
            {
                staticFrame.Mutate(ctx => ctx.DrawImage(currentImage, new Point(0, 0), 1f));
                var staticMetadata = staticFrame.Frames.RootFrame.Metadata.GetGifMetadata();
                staticMetadata.FrameDelay = staticFrameDelay;
                gif.Frames.AddFrame(staticFrame.Frames.RootFrame);
            }

            for (var step = 0; step < slideSteps; step++)
            {
                var offset = step * stepSize;
                using var frame = new Image<Rgba32>(currentImage.Width, currentImage.Height);

                if (-offset < frame.Width && currentImage.Width - offset > 0)
                {
                    frame.Mutate(ctx => ctx.DrawImage(currentImage, new Point(-offset, 0), 1f));
                }

                if (currentImage.Width - offset < frame.Width && offset > 0)
                {
                    frame.Mutate(ctx => ctx.DrawImage(nextImage, new Point(currentImage.Width - offset, 0), 1f));
                }

                var metadata = frame.Frames.RootFrame.Metadata.GetGifMetadata();
                metadata.FrameDelay = frameDelay;
                gif.Frames.AddFrame(frame.Frames.RootFrame);
            }
        }
    }
}