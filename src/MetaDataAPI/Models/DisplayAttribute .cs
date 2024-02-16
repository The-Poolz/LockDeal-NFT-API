using MetaDataAPI.Models.Response;
using MetaDataAPI.Models.Types;

namespace MetaDataAPI.Models;

[AttributeUsage(AttributeTargets.Property)]
public class DisplayAttribute(DisplayType DisplayType, string? DisplayName = null) : Attribute
{
    public bool TryGetErc721Attribute(string propertyName, object? value, out Erc721Attribute erc721Attribute)
    {
        if (DisplayType == DisplayType.Ignore ||
            value == null ||
            (string.IsNullOrEmpty(DisplayName) && string.IsNullOrEmpty(propertyName)))
        {
            erc721Attribute = null!;
            return false;
        }
        if (DisplayType == DisplayType.Date)
            erc721Attribute = new Erc721Attribute(DisplayName ?? propertyName, value, DisplayType);
        else
            erc721Attribute = new Erc721Attribute(DisplayName ?? propertyName, value);
        return true;
    }
}
