using Newtonsoft.Json;

namespace MetaDataAPI.Models.Response;

public class Erc721Metadata
{
    public Erc721Metadata(BasePoolInfo poolInfo)
    {
        Name = "Lock Deal NFT Pool: " + poolInfo.PoolId;
        Description = "Poolz.finance Lock Deal NFT for token Vesting Pool: " + poolInfo.PoolId;
        Image = @"https://nft.poolz.finance/test/image?id=" + poolInfo.PoolId;
        Attributes = poolInfo.Attributes.ToList();
    }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("image")]
    public string Image { get; set; }

    [JsonProperty("attributes")]
    public List<Attribute> Attributes { get; set; }
}
