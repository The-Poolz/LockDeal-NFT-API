using SixLabors.ImageSharp;

namespace ImageAPI.Processing;

public static class ElementPositions
{
    public static PointF ProviderNameCoordinates => new(672, 52);
    public static PointF PoolIdCoordinates => new(981, 52);
    public static PointF LeftAmountTextCoordinates => new(672, 274);
    public static PointF LeftAmountCoordinates => new(672, 298);
    public static PointF StartTimeTextCoordinates => new(48, 274);
    public static PointF StartTimeDateCoordinates => new(48, 298);
    public static PointF StartTimeTimeCoordinates => new(48, 310);
    public static PointF FinishTimeTextCoordinates => new(276, 274);
    public static PointF FinishTimeDateCoordinates => new(276, 298);
    public static PointF FinishTimeTimeCoordinates => new(276, 310);
}