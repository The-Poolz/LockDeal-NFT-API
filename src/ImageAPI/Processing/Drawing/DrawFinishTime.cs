using MetaDataAPI.Utils;
using SixLabors.ImageSharp;
using System.Globalization;

namespace ImageAPI.Processing.Drawing;

public class DrawFinishTime : ToDrawing
{
    public DrawFinishTime(object finishTime)
        : base(
            text: TimeUtils.FromUnixTimestamp((long)finishTime).ToString(CultureInfo.InvariantCulture),
            coordinates: new PointF(ImageSize.Width - 730, ImageSize.Height - 290),
            fontSize: DateTimeFontSize
        )
    { }
}