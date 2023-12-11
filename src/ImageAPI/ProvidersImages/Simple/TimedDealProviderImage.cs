using SixLabors.Fonts;
using SixLabors.ImageSharp;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.DynamoDb;

namespace ImageAPI.ProvidersImages.Simple;

public class TimedDealProviderImage : ProviderImage
{
    public override IDictionary<string, PointF> AttributeCoordinates => new Dictionary<string, PointF>
    {
        { "LeftAmount", new PointF(BackgroundImage.Width - 400, BackgroundImage.Height - 290) },
        { "StartTime", new PointF(BackgroundImage.Width - 1030, BackgroundImage.Height - 290) },
        { "FinishTime", new PointF(BackgroundImage.Width - 730, BackgroundImage.Height - 290) },
        //{ "Collection", new PointF(BackgroundImage.Width / 2f, BackgroundImage.Height / 5f) }
    };

    public override IDictionary<string, PointF> TextCoordinates => new Dictionary<string, PointF>
    {
        { "Left", new PointF(BackgroundImage.Width - 400, BackgroundImage.Height - 330) },
        { "Start", new PointF(BackgroundImage.Width - 1030, BackgroundImage.Height - 330) },
        { "Finish", new PointF(BackgroundImage.Width - 730, BackgroundImage.Height - 330) }
    };

    public TimedDealProviderImage(Image backgroundImage, Font font, IReadOnlyList<DynamoDbItem> dynamoDbItems)
        : base(nameof(TimedDealProvider), backgroundImage, font, dynamoDbItems[0])
    { }
}