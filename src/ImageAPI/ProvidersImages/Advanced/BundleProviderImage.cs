﻿using SixLabors.ImageSharp;
using MetaDataAPI.Models.Response;
using System.Text.RegularExpressions;
using ImageAPI.Utils;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Fonts;
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
        : base(backgroundImage)
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
            .Select(includedAttributes => ProviderImageFactory.Create(backgroundImage, font, includedAttributes))
            .Select(providerImage => providerImage.Image)
            .ToArray();

        var imageProcessor = new ImageProcessor(backgroundImage, font);
        foreach (var attribute in attributes)
        {
            var coordinates = GetCoordinates(attribute.TraitType);
            if (coordinates == null)
                continue;

            var options = imageProcessor.CreateTextOptions((PointF)coordinates);
            imageProcessor.DrawText(attribute, options);
        }

        var imageList = new List<Image>(bundleImages)
        {
            imageProcessor.Image
        };

        const int frameDelay = 100;

        using var gif = imageProcessor.Image;

        var gifMetaData = gif.Metadata.GetGifMetadata();
        gifMetaData.RepeatCount = 5;

        var metadata = gif.Frames.RootFrame.Metadata.GetGifMetadata();
        metadata.FrameDelay = frameDelay;
        for (var i = 0; i < imageList.Count; i++)
        {
            metadata = imageList[i].Frames.RootFrame.Metadata.GetGifMetadata();
            metadata.FrameDelay = frameDelay;

            gif.Frames.AddFrame(imageList[i].Frames.RootFrame);
        }

        Image = gif.Clone(ctx => {});

        gif.SaveAsGif("animated.gif");
    }
}