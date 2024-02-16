using Newtonsoft.Json;

namespace MetaDataAPI.Models.Response;

public class Erc721Metadata(string name, string description, string image, IEnumerable<Erc721Attribute> attributes)
{
    [JsonProperty("name")]
    public string Name { get; } = name;

    [JsonProperty("description")]
    public string Description { get; } = description;

    [JsonProperty("image")]
    public string Image { get; } = image;

    [JsonProperty("attributes")]
    public IEnumerable<Erc721Attribute> Attributes { get; } = attributes;
}
