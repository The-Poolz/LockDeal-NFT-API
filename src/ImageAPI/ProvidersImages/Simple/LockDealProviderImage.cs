﻿using ImageAPI.Processing;
using SixLabors.ImageSharp;
using MetaDataAPI.Models.DynamoDb;

namespace ImageAPI.ProvidersImages.Simple;

public class LockDealProviderImage : DealProviderImage
{
    public LockDealProviderImage(IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(dynamoDbItems)
    { }

    protected override IEnumerable<Action<Image>> DrawingActions()
    {
        foreach (var action in base.DrawingActions())
        {
            yield return action;
        }
        yield return drawOn => drawOn.DrawStartTime(GetAttributeValue("StartTime"));
    }
}