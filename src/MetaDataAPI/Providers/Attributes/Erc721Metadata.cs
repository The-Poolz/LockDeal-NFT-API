using Newtonsoft.Json;

namespace MetaDataAPI.Providers.Attributes;

public class Erc721Metadata(string name, string description, string image, IEnumerable<Erc721MetadataItem> attributes)
{
    [JsonProperty("name", Order = 1)]
    public string Name { get; set; } = name;

    [JsonProperty("description", Order = 2)]
    public string Description { get; set; } = description;

    [JsonProperty("image", Order = 3)]
    public string Image { get; set; } = image;

    [JsonProperty("type", Order = 4)]
    public string Type { get; set; } = "image/jpeg";

    [JsonProperty("properties", Order = 5)] 
    public Erc721Properties Properties = new();

    [JsonProperty("attributes", Order = 6)]
    public IEnumerable<Erc721MetadataItem> Attributes { get; set; } = attributes;
}