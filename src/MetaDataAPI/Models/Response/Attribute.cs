using Newtonsoft.Json;

namespace MetaDataAPI.Models.Response;

public class Attribute
{
    [JsonProperty("display_type", NullValueHandling = NullValueHandling.Ignore)]
    public string? DisplayType { get; set; }

    [JsonProperty("trait_type")]
    public string TraitType { get; set; }

    [JsonProperty("value")]
    public object Value { get; set; }

    public Attribute(string traitType, object value, string? displayType = null)
    {
        TraitType = traitType;
        Value = value;
        DisplayType = displayType;
    }
}