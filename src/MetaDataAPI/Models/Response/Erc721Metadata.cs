using Newtonsoft.Json;

namespace MetaDataAPI.Models.Response;

public class Erc721Metadata
{
    [JsonProperty("name")]
    public string Name { get; }

    [JsonProperty("description")]
    public string Description { get; }

    [JsonProperty("image")]
    public string Image { get; }

    [JsonProperty("attributes")]
    public IEnumerable<Erc721Attribute> Attributes { get; }

    public Erc721Metadata(string name, string description, string image, IEnumerable<Erc721Attribute> attributes)
    {
        Name = name;
        Description = description;
        Image = image;
        Attributes = attributes;
    }
}
