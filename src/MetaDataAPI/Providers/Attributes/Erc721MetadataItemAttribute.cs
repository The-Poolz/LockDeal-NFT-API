namespace MetaDataAPI.Providers.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class Erc721MetadataItemAttribute : Attribute
{
    public string TraitType { get; set; }
    public DisplayType DisplayType { get; set; }

    public Erc721MetadataItemAttribute(string traitType, DisplayType displayType)
    {
        TraitType = traitType;
        DisplayType = displayType;
    }
}