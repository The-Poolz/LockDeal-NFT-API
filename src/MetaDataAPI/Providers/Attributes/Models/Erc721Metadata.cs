using Newtonsoft.Json;

namespace MetaDataAPI.Providers.Attributes.Models;

public class Erc721Metadata
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("image")]
    public string Image { get; set; }

    [JsonProperty("attributes")]
    public IEnumerable<Erc721Attribute> Attributes { get; set; }

    public Erc721Metadata(string name, string description, string image, IEnumerable<Erc721Attribute> attributes)
    {
        Name = name;
        Description = description;
        Image = image;
        Attributes = attributes;
    }
}