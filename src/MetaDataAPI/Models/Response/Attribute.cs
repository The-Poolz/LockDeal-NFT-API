using Newtonsoft.Json;

namespace MetaDataAPI.Models.Response;

public class Attribute
{
    [JsonProperty("trait_type")]
    public string TraitType { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }

    public Attribute(string traitType, string value)
    {
        TraitType = traitType;
        Value = value;
    }
}