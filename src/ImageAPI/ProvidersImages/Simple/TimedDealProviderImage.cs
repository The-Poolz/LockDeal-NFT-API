﻿using SixLabors.Fonts;
using SixLabors.ImageSharp;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.DynamoDb;

namespace ImageAPI.ProvidersImages.Simple;

public class TimedDealProviderImage : ProviderImage
{
    public override IDictionary<string, PointF> AttributeCoordinates => new Dictionary<string, PointF>
    {
        { "LeftAmount", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 2f) },
        { "StartTime", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 3f) },
        { "FinishTime", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 4f) },
        { "Collection", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 5f) }
    };

    public override IDictionary<string, PointF> TextCoordinates { get; }

    public TimedDealProviderImage(Image backgroundImage, Font font, IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(nameof(TimedDealProvider), backgroundImage, font, dynamoDbItems[0])
    { }
}