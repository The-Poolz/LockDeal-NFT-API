using System.Numerics;
using SixLabors.ImageSharp;
using static ImageAPI.Processing.DrawingSettings;

namespace ImageAPI.Processing.Drawing;

public class DrawPoolId : ToDrawing
{
    private readonly BigInteger poolId;

    public DrawPoolId(BigInteger poolId)
    {
        this.poolId = poolId;
    }

    public override Image Draw(Image drawOn)
    {
        return Draw(drawOn, new ToDrawParameters($"#{poolId}", PoolId.Position, PoolId.FontSize));
    }
}