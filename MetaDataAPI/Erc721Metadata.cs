using System.Text.Json.Serialization;

namespace MetaDataAPI;

public class Erc721Metadata
{
    public Erc721Metadata(BasePoolInfo poolInfo)
    {
        name = "Lock Deal NFT Pool: " + poolInfo.PoolId;
        description = "Poolz.finance Lock Deal NFT for token Vesting Pool: " + poolInfo.PoolId;
        image = @"https://nft.poolz.finance/test/image?id=" + poolInfo.PoolId;
        attributes = new List<Attribute>();

        foreach (var param in poolInfo.ParamsDict)
        {
            attributes.Add(new Attribute()
            {
                trait_type = param.Key,
                value = param.Value.ToString()
            });
        }

    }
    [JsonPropertyName("name")]
    public string name { get; set; }
    [JsonPropertyName("description")]
    public string description { get; set; }
    [JsonPropertyName("image")]
    public string image { get; set; }
    [JsonPropertyName("attributes")]
    public List<Attribute> attributes { get; set; }

    public class Attribute
    {
        public string trait_type { get; set; }
        public string value { get; set; }
    }
}
