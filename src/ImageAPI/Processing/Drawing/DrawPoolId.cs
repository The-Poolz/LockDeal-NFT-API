using System.Numerics;
using ImageAPI.Processing.Drawing.Options;
using SixLabors.ImageSharp;
using static ImageAPI.Settings.DrawingSettings;

namespace ImageAPI.Processing.Drawing;

public class DrawPoolId : ToDrawing
{
    private readonly BigInteger poolId;

    public DrawPoolId(BigInteger poolId)
    {
        this.poolId = poolId;
    }

    public override void Draw(Image drawOn)
    {
        Draw(drawOn, new DrawOptions($"#{poolId}", PoolId.Position, PoolId.FontSize));
    }
}