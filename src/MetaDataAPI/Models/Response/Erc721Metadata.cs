using Newtonsoft.Json;

namespace MetaDataAPI.Models.Response;

public class Erc721Metadata
{
    public Erc721Metadata(string name, string description, string image, List<Erc721Attribute> attributes)
    {
        Name = name;
        Description = description;
        Image = image;
        Attributes = attributes;
    }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("image")]
    public string Image { get; set; }

    [JsonProperty("attributes")]
    public List<Erc721Attribute> Attributes { get; set; }
}
