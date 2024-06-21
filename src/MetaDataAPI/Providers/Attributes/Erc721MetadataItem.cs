using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MetaDataAPI.Providers.Attributes;

public class Erc721MetadataItem
{
    [JsonConverter(typeof(StringEnumConverter))]
    [JsonProperty("display_type", NullValueHandling = NullValueHandling.Ignore)]
    public DisplayType DisplayType { get; set; }

    [JsonProperty("trait_type")]
    public string TraitType { get; set; }

    [JsonProperty("value")]
    public object Value { get; set; }

    public Erc721MetadataItem(
        string traitType,
        object value,
        DisplayType displayType
    )
    {
        TraitType = traitType;
        Value = value;
        DisplayType = displayType;
    }
}