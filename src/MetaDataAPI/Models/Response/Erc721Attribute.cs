using System.Numerics;
using Newtonsoft.Json;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response.Converters;

namespace MetaDataAPI.Models.Response;

//[JsonConverter(typeof(Erc721AttributeConverter))]
public class Erc721Attribute
{
    [JsonProperty("display_type", NullValueHandling = NullValueHandling.Ignore)]
    public DisplayType DisplayType { get; }

    [JsonProperty("trait_type")]
    public string TraitType { get; private set; }

    [JsonProperty("value")]
    public object Value { get; }

    public Erc721Attribute(
        string traitType,
        object value,
        DisplayType displayType = DisplayType.String
    )
    {
        TraitType = traitType;
        Value = value;
        DisplayType = displayType;
    }

    public Erc721Attribute IncludeUnderscoreForTraitType(BigInteger poolId)
    {
        TraitType += "_" + poolId;
        return this;
    }
}
