namespace MetaDataAPI.Models.Types;

public static class DisplayTypeExtensions
{
    public static string ToLowerString(this DisplayType displayType) =>
        displayType.ToString().ToLower();
}