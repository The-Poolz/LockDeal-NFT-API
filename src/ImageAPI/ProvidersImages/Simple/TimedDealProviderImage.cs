﻿using SixLabors.Fonts;
using SixLabors.ImageSharp;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.DynamoDb;

namespace ImageAPI.ProvidersImages.Simple;

public class TimedDealProviderImage : ProviderImage
{
    public override string ProviderName => nameof(TimedDealProvider);
    public override Image Image { get; }
    public override IDictionary<string, PointF> Coordinates => new Dictionary<string, PointF>
    {
        { "LeftAmount", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 2f) },
        { "StartTime", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 3f) },
        { "FinishTime", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 4f) },
        { "Collection", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 5f) }
    };

    public TimedDealProviderImage(Image backgroundImage, Font font, IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(backgroundImage, font)
    {
        Image = DrawProviderName();
        Image = DrawAttributes(dynamoDbItems[0]);
    }
}