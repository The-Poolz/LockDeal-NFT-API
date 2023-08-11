using Newtonsoft.Json;

namespace MetaDataAPI.Models.Response;

public class Erc721Attribute
{
    [JsonProperty("max_value", NullValueHandling = NullValueHandling.Ignore)]
    public int? MaxValue { get; }

    [JsonProperty("display_type", NullValueHandling = NullValueHandling.Ignore)]
    public string? DisplayType { get; }

    [JsonProperty("trait_type")]
    public string TraitType { get; }

    [JsonProperty("value")]
    public object Value { get; }

    public Erc721Attribute(string traitType, object value, string? displayType = null, int? maxValue = null)
    {
        TraitType = traitType;
        Value = value;
        DisplayType = displayType;
        MaxValue = maxValue;
    }
}
