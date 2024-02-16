using MetaDataAPI.Utils;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models;
using MetaDataAPI.Models.DynamoDb;
using poolz.finance.csharp.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class LockDealProvider : DealProvider
{
    public override string ToString() => $"securely locks {LeftAmount} units of the asset Until {StartDateTime}";
    public override string ProviderName => nameof(LockDealProvider);
    public override string Description =>
        $"This NFT securely locks {LeftAmount} units of the asset {PoolInfo.Token}. " +
        $"Access to these assets will commence on the designated start time of {StartDateTime}.";
    [Display(DisplayType.Date)]
    public uint StartTime { get; }
    public DateTime StartDateTime => TimeUtils.FromUnixTimestamp(StartTime);
    public override List<DynamoDbItem> DynamoDbAttributes => new()
    {
        new DynamoDbItem(ProviderName, PoolInfo, new List<Erc721Attribute>
        {
            new("StartTime", StartTime, DisplayType.Date),
            new("Collection", Collection),
            new("LeftAmount", LeftAmount)
        })
    };

    public LockDealProvider(BasePoolInfo[] basePoolInfo)
        : base(basePoolInfo)
    {
        StartTime = (uint)basePoolInfo.FirstOrDefault()!.Params[1];
    }
}