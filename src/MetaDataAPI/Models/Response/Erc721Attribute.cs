using Newtonsoft.Json;
using MetaDataAPI.Models.Types;
using Newtonsoft.Json.Converters;

namespace MetaDataAPI.Models.Response;

public class Erc721Attribute
{
    [JsonConverter(typeof(StringEnumConverter))]
    [JsonProperty("display_type", NullValueHandling = NullValueHandling.Ignore)]
    public DisplayType DisplayType { get; }

    [JsonProperty("trait_type")]
    public string TraitType { get; }

    [JsonProperty("value")]
    public object Value { get; }

    public Erc721Attribute(
        string traitType,
        object value,
        DisplayType displayType = DisplayType.None
    )
    {
        TraitType = traitType;
        Value = value;
        DisplayType = displayType;
    }
}
