using MetaDataAPI.Utils;
using SixLabors.ImageSharp;
using System.Globalization;
using static ImageAPI.Processing.DrawingSettings;

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
        drawOn = Draw(drawOn, new ToDrawParameters("Start Time", StartTime.HeaderPosition, HeaderFontSize));
        drawOn = Draw(drawOn, new ToDrawParameters(startTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), StartTime.DatePosition, StartTime.FontSize));
        return Draw(drawOn, new ToDrawParameters(startTime.ToString("HH:mm:ss", CultureInfo.InvariantCulture), StartTime.TimePosition, StartTime.FontSize));
    }
}