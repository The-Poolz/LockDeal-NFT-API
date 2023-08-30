using MetaDataAPI.Models.Types;
using Newtonsoft.Json;

namespace MetaDataAPI.Models.Response;

public class Erc721Metadata
{
    public Erc721Metadata(BasePoolInfo poolInfo)
    {
        Name = "Lock Deal NFT Pool: " + poolInfo.PoolId;
        Attributes = poolInfo.Attributes.ToList();
        Image = @"https://nft.poolz.finance/test/image?id=" + poolInfo.PoolId;
        Description = GetDescription(poolInfo);
    }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("image")]
    public string Image { get; set; }

    [JsonProperty("attributes")]
    public List<Erc721Attribute> Attributes { get; set; }

    private string GetDescription(BasePoolInfo poolInfo)
    {
        switch (poolInfo.Provider.Name)
        {
            case ProviderName.Deal:
                return $"This NFT represents immediate access to {Attributes[0].Value} units of the specified asset {poolInfo.Token}.";
            case ProviderName.Lock:
                return $"This NFT securely locks {Attributes[0].Value} units of the asset {poolInfo.Token}. Access to these assets will commence on the designated start time of {Attributes[1].Value}.";
            case ProviderName.Timed:
                return $"This NFT governs a time-locked pool containing {Attributes[0].Value} units of the asset {poolInfo.Token}. Withdrawals are permitted in a linear fashion beginning at {Attributes[1].Value}, culminating in full access at {Attributes[2].Value}.";
            default:
                throw new InvalidOperationException($"Not found description for '{poolInfo.Provider.Name}' provider.");
        }
    }
}
