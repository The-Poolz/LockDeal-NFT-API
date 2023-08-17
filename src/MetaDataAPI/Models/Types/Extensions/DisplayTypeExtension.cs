namespace MetaDataAPI.Models.Types.Extensions;

public static class DisplayTypeExtension
{
    public static string ToLowerString(this DisplayType displayType) =>
        displayType.ToString().ToLower();
}