using MetaDataAPI.Utils;
using SixLabors.ImageSharp;
using System.Globalization;
using ImageAPI.Processing.Drawing.Options;
using static ImageAPI.Settings.DrawingSettings;

namespace ImageAPI.Processing.Drawing;

public class DrawStartTime : ToDrawing
{
    private readonly DateTime startTime;

    public DrawStartTime(object startTime)
    {
        this.startTime = TimeUtils.FromUnixTimestamp((long)startTime);
    }

    public override void Draw(Image drawOn)
    {
        Draw(drawOn, new DrawOptions("Start Time", StartTime.HeaderPosition, HeaderFontSize));
        Draw(drawOn, new DrawOptions(startTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), StartTime.DatePosition, StartTime.FontSize));
        Draw(drawOn, new DrawOptions(startTime.ToString("HH:mm:ss", CultureInfo.InvariantCulture), StartTime.TimePosition, StartTime.FontSize));
    }
}