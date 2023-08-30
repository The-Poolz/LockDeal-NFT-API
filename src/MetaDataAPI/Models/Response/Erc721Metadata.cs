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

    public string GetDescription(BasePoolInfo poolInfo)
    {
        switch (poolInfo.Provider.Name)
        {
            case ProviderName.Deal:
                return DealDescription(Attributes[0].Value, poolInfo.Token);
            case ProviderName.Lock:
                return LockDescription(Attributes[0].Value, poolInfo.Token, Attributes[1].Value);
            case ProviderName.Timed:
                return TimedDescription(Attributes[0].Value, poolInfo.Token, Attributes[1].Value, Attributes[2].Value);
            default:
                throw new InvalidOperationException($"Not found description for '{poolInfo.Provider.Name}' provider.");
        }
    }

    public static string DealDescription(object leftAmonut, string token) =>
        $"This NFT represents immediate access to {leftAmonut} units of the specified asset {token}.";

    public static string LockDescription(object leftAmonut, string token, object startTime) =>
        $"This NFT securely locks {leftAmonut} units of the asset {token}. Access to these assets will commence on the designated start time of {startTime}.";

    public static string TimedDescription(object leftAmonut, string token, object startTime, object finishTime) =>
        $"This NFT governs a time-locked pool containing {leftAmonut} units of the asset {token}. Withdrawals are permitted in a linear fashion beginning at {startTime}, culminating in full access at {finishTime}.";
}
