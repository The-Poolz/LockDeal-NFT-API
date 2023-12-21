using MetaDataAPI.Utils;
using SixLabors.ImageSharp;
using System.Globalization;
using ImageAPI.Processing.Drawing.Options;
using static ImageAPI.Settings.DrawingSettings;

namespace ImageAPI.Processing.Drawing;

public class DrawFinishTime : ToDrawing
{
    private readonly DateTime finishTime;

    public DrawFinishTime(object finishTime)
    {
        this.finishTime = TimeUtils.FromUnixTimestamp((long)finishTime);
    }

    public override void Draw(Image drawOn)
    {
        Draw(drawOn, new DrawOptions("Finish Time", FinishTime.HeaderPosition, HeaderFontSize));
        Draw(drawOn, new DrawOptions(finishTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), FinishTime.DatePosition, FinishTime.FontSize));
        Draw(drawOn, new DrawOptions(finishTime.ToString("HH:mm:ss", CultureInfo.InvariantCulture), FinishTime.TimePosition, FinishTime.FontSize));
    }
}