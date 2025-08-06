using Newtonsoft.Json;
using EnvironmentManager.Extensions;

namespace MetaDataAPI.Providers.Attributes;

public class Erc721Metadata(string name, string description, string image, IEnumerable<Erc721MetadataItem> attributes)
{
    [JsonProperty("external_url")]
    public string ExternalUrl { get; set; } = Environments.EXTERNAL_URL.Get();

    [JsonProperty("name")]
    public string Name { get; set; } = name;

    [JsonProperty("description")]
    public string Description { get; set; } = description;

    [JsonProperty("image")]
    public string Image { get; set; } = image;

    [JsonProperty("type")]
    public string Type { get; set; } = "image/jpeg";

    [JsonProperty("attributes")]
    public IEnumerable<Erc721MetadataItem> Attributes { get; set; } = attributes;
}