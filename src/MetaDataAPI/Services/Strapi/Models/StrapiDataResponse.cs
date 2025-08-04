using Newtonsoft.Json;
using Poolz.Finance.CSharp.Strapi;

namespace MetaDataAPI.Services.Strapi.Models;

public record StrapiDataResponse(
    [JsonProperty("contractsOnChains")]
    ICollection<ContractsOnChain> ContractOnChains
);