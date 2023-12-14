using SixLabors.ImageSharp;

namespace ImageAPI.Processing;

public static class DrawingSettings
{
    public static float HeaderFontSize => 16f;

    public struct ProviderName
    {
        public static float FontSize => 24f;
        public static PointF Position => new(672, 52);
    }

    public struct PoolId
    {
        public static float FontSize => 24f;
        public static PointF Position => new(981, 52);
    }

    public struct LeftAmount
    {
        public static float FontSize => 64f;
        public static PointF Position => new(672, 298);
        public static PointF HeaderPosition => new(672, 274);
    }

    public struct StartTime
    {
        public static float FontSize => 32f;
        public static PointF DatePosition => new(48, 298);
        public static PointF TimePosition => new(48, 340);
        public static PointF HeaderPosition => new(48, 274);
    }

    public struct FinishTime
    {
        public static float FontSize => 32f;
        public static PointF DatePosition => new(276, 298);
        public static PointF TimePosition => new(276, 340);
        public static PointF HeaderPosition => new(276, 274);
    }
}