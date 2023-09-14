using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class DealProvider : Provider
{
    public override string ProviderName => nameof(DealProvider);
    public override string Description =>
        $"This NFT represents immediate access to {LeftAmount} units of the specified asset {PoolInfo.Token}.";
    public override IEnumerable<Erc721Attribute> ProviderAttributes => new Erc721Attribute[]
    {
        new("LeftAmount", LeftAmount, DisplayType.Number)
    };

    public decimal LeftAmount { get; }

    public DealProvider(BasePoolInfo basePoolInfo)
        : base(basePoolInfo)
    {
        var converter = new ConvertWei(basePoolInfo.Token.Decimals);
        LeftAmount = converter.WeiToEth(basePoolInfo.Params[0]);
    }
}