using MetaDataAPI.Models.Response;
using MetaDataAPI.Models.Types;

namespace MetaDataAPI.Models;

[AttributeUsage(AttributeTargets.Property)]
public class DisplayAttribute : Attribute
{
    public DisplayType DisplayType { get; }
    public string? DisplayName { get; }
    public DisplayAttribute(DisplayType displayType, string? displayName = null)
    {
        DisplayType = displayType;
        DisplayName = displayName;
    }
    public bool TryGetErc721Attribute(string propertyName, object? value, out Erc721Attribute erc721Attribute)
    {
        if (DisplayType == DisplayType.Ignore ||
            value == null ||
            string.IsNullOrEmpty(DisplayName) && string.IsNullOrEmpty(propertyName))
        {
            erc721Attribute = null!;
            return false;
        }
        erc721Attribute = new Erc721Attribute(DisplayName ?? propertyName, value, DisplayType);
        return true;
    }
}
