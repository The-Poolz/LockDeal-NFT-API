using SixLabors.ImageSharp;

namespace ImageAPI.ProvidersImages;

public abstract class ProviderImage
{
    public abstract Image Image { get; }
    public abstract IDictionary<string, PointF> Coordinates { get; }

    protected PointF GetCoordinates(string traitType)
    {
        if (Coordinates.TryGetValue(traitType, out var coordinates))
        {
            return coordinates;
        }

        throw new Exception($"Coordinates for trait_type '{traitType}' are not defined.");
    }
}