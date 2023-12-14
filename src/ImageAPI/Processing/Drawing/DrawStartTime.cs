using MetaDataAPI.Utils;
using SixLabors.ImageSharp;
using System.Globalization;

namespace ImageAPI.Processing.Drawing;

public class DrawStartTime : ToDrawing
{
    private readonly DateTime startTime;

    public DrawStartTime(object startTime)
    {
        this.startTime = TimeUtils.FromUnixTimestamp((long)startTime);
    }

    public override Image Draw(Image drawOn)
    {
        drawOn = Draw(drawOn, startTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), new PointF(ImageSize.Width - 1030, ImageSize.Height - 290), DateTimeFontSize);
        return Draw(drawOn, startTime.ToString("HH:mm:ss", CultureInfo.InvariantCulture), new PointF(ImageSize.Width - 1030, ImageSize.Height - 260), DateTimeFontSize);
    }
}