using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class LockDealProvider : Provider
{
    public override string ProviderName => nameof(LockDealProvider);
    public override string Description =>
        $"This NFT securely locks {LeftAmount} units of the asset {PoolInfo.Token}. Access to these assets will commence on the designated start time of {TimeUtils.FromUnixTimestamp(StartTime)}.";
    public override IEnumerable<Erc721Attribute> ProviderAttributes => new[]
    {
        new Erc721Attribute("LeftAmount", LeftAmount, DisplayType.Number),
        new Erc721Attribute("StartTime", StartTime, DisplayType.Date),
        new Erc721Attribute("VaultId", PoolInfo.VaultId, DisplayType.Number),
        TokenNameAttribute
    };

    public decimal LeftAmount { get; }
    public uint StartTime { get; }

    public LockDealProvider(BasePoolInfo basePoolInfo)
        : base(basePoolInfo)
    {
        var converter = new ConvertWei(basePoolInfo.Token.Decimals);
        LeftAmount = converter.WeiToEth(basePoolInfo.Params[0]);
        StartTime = (uint)basePoolInfo.Params[1];
    }
}