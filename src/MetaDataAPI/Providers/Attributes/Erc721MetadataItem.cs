using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MetaDataAPI.Providers.Attributes;

public class Erc721MetadataItem(object value, string traitType, DisplayType? displayType)
{
    [JsonProperty("value")]
    public object Value { get; set; } = value;

    [JsonProperty("trait_type")]
    public string TraitType { get; set; } = traitType;

    [JsonConverter(typeof(StringEnumConverter))]
    [JsonProperty("display_type", NullValueHandling = NullValueHandling.Ignore)]
    public DisplayType? DisplayType { get; set; } = displayType;
}