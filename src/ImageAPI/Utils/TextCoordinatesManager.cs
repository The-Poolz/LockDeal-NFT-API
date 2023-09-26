using SixLabors.ImageSharp;

namespace ImageAPI.Utils;

public class TextCoordinatesManager
{
    private readonly Dictionary<string, PointF> traitTypeToCoordinates;

    public TextCoordinatesManager()
    {
        traitTypeToCoordinates = new Dictionary<string, PointF>
        {
            { "ProviderName", new PointF(92, 52) },
            { "LeftAmount", new PointF(610, 300) }
        };
    }

    public PointF GetCoordinatesForTraitType(string traitType)
    {
        if (traitTypeToCoordinates.TryGetValue(traitType, out var coordinates))
        {
            return coordinates;
        }

        throw new Exception($"Coordinates for trait_type '{traitType}' are not defined.");
    }
}