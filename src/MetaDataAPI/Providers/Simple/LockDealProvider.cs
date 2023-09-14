using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class LockDealProvider : Provider
{
    public override string ProviderName => nameof(LockDealProvider);
    public override IEnumerable<Erc721Attribute> ProviderAttributes => new Erc721Attribute[]
    {
        new("LeftAmount", LeftAmount, DisplayType.Number),
        new("StartTime", StartTime, DisplayType.Date)
    };

    public decimal LeftAmount { get; }
    public uint StartTime { get; }

    public LockDealProvider(BasePoolInfo basePoolInfo)
        : base(basePoolInfo)
    {
        var converter = new ConvertWei(basePoolInfo.Token.Decimals);
        LeftAmount = converter.WeiToEth(basePoolInfo.Params[0]);
        StartTime = (uint)new ConvertWei(0).WeiToEth(basePoolInfo.Params[1]);
    }

    public override string GetDescription() =>
        $"This NFT securely locks {LeftAmount} units of the asset {PoolInfo.Token}. Access to these assets will commence on the designated start time of {TimeUtils.FromUnixTimestamp(StartTime)}.";
}