using System.Numerics;
using Newtonsoft.Json;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Types.Extensions;

namespace MetaDataAPI.Models.Response;

public class Erc721Attribute
{
    [JsonProperty("display_type", NullValueHandling = NullValueHandling.Ignore)]
    public string? DisplayType { get; }

    [JsonProperty("trait_type")]
    public string TraitType { get; private set; }

    [JsonProperty("value")]
    public object Value { get; }

    public Erc721Attribute(
        string traitType,
        object value,
        DisplayType displayType = Types.DisplayType.String,
    )
    {
        TraitType = traitType;
        Value = value;

        // For string values don't need to set DisplayType, because of this here ternary expression.
        DisplayType = displayType == Types.DisplayType.String ? null : displayType.ToLowerString();
    }

    public Erc721Attribute IncludeUnderscoreForTraitType(BigInteger poolId)
    {
        TraitType += "_" + poolId;
        return this;
    }
}
