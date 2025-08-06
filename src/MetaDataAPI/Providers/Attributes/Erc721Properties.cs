using Newtonsoft.Json;
using EnvironmentManager.Extensions;

namespace MetaDataAPI.Providers.Attributes;

public class Erc721Properties
{
    [JsonProperty("external_url")]
    public string ExternalUrl { get; set; } = Environments.EXTERNAL_URL.Get();
}