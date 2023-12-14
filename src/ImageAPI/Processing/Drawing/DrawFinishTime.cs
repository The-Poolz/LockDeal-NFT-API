using MetaDataAPI.Utils;
using SixLabors.ImageSharp;
using System.Globalization;

namespace ImageAPI.Processing.Drawing;

public class DrawFinishTime : ToDrawing
{
    private readonly DateTime finishTime;

    public DrawFinishTime(object finishTime)
    {
        this.finishTime = TimeUtils.FromUnixTimestamp((long)finishTime);
    }

    public override Image Draw(Image drawOn)
    {
        drawOn = Draw(drawOn, "Finish Time", new PointF(ImageSize.Width - 730, ImageSize.Height - 330), TextFontSize);
        drawOn = Draw(drawOn, finishTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), new PointF(ImageSize.Width - 730, ImageSize.Height - 290), DateTimeFontSize);
        return Draw(drawOn, finishTime.ToString("HH:mm:ss", CultureInfo.InvariantCulture), new PointF(ImageSize.Width - 730, ImageSize.Height - 260), DateTimeFontSize);
    }
}