using MetaDataAPI.Utils;
using SixLabors.ImageSharp;
using System.Globalization;
using static ImageAPI.Processing.DrawingSettings;

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
        drawOn = Draw(drawOn, "Finish Time", FinishTime.HeaderPosition, HeaderFontSize);
        drawOn = Draw(drawOn, finishTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), FinishTime.DatePosition, FinishTime.FontSize);
        return Draw(drawOn, finishTime.ToString("HH:mm:ss", CultureInfo.InvariantCulture), FinishTime.TimePosition, FinishTime.FontSize);
    }
}