using MetaDataAPI.Providers.Attributes.Models;

namespace MetaDataAPI.Providers.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class Erc721AttributeAttribute : Attribute
{
    public string TraitType { get; set; }
    public DisplayType DisplayType { get; set; }

    public Erc721AttributeAttribute(string traitType, DisplayType displayType)
    {
        TraitType = traitType;
        DisplayType = displayType;
    }
}