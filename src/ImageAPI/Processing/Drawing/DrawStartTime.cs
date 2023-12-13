using MetaDataAPI.Utils;
using SixLabors.ImageSharp;
using System.Globalization;

namespace ImageAPI.Processing.Drawing;

public class DrawStartTime : ToDrawing
{
    public DrawStartTime(object startTime)
        : base(
            text: TimeUtils.FromUnixTimestamp((long)startTime).ToString(CultureInfo.InvariantCulture),
            coordinates: new PointF(ImageSize.Width - 1030, ImageSize.Height - 290),
            fontSize: DateTimeFontSize
        )
    { }
}