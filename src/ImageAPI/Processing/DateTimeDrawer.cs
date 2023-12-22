using MetaDataAPI.Utils;
using System.Globalization;
using SixLabors.ImageSharp;

namespace ImageAPI.Processing;

public static class DateTimeDrawer
{
    public static void DrawStartTime(this Image drawOn, object startTime)
    {
        drawOn.DrawHeader("Start Time", new PointF(48, 274));
        drawOn.DrawDate(startTime, new PointF(48, 298), 32f);
        drawOn.DrawTime(startTime, new PointF(48, 340), 32f);
    }

    public static void DrawFinishTime(this Image drawOn, object finishTime)
    {
        drawOn.DrawHeader("Finish Time", new PointF(276, 274));
        drawOn.DrawDate(finishTime, new PointF(276, 298), 32f);
        drawOn.DrawTime(finishTime, new PointF(276, 340), 32f);
    }

    public static void DrawDate(this Image drawOn, object dateTime, PointF coordinates, float fontSize)
    {
        drawOn.DrawText(TimeUtils.FromUnixTimestamp((long)dateTime).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), coordinates, fontSize);
    }

    public static void DrawTime(this Image drawOn, object dateTime, PointF coordinates, float fontSize)
    {
        drawOn.DrawText(TimeUtils.FromUnixTimestamp((long)dateTime).ToString("HH:mm:ss", CultureInfo.InvariantCulture), coordinates, fontSize);
    }
}