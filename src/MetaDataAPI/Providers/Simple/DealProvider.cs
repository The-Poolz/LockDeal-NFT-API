using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class DealProvider : Provider
{
    public decimal LeftAmount { get; }

    public DealProvider(BasePoolInfo basePoolInfo) : base(basePoolInfo)
    {
        var converter = new ConvertWei(basePoolInfo.Token.Decimals);
        LeftAmount = converter.WeiToEth(basePoolInfo.Params[0]);
        AddAttributes(nameof(DealProvider));
    }

    public override List<Erc721Attribute> GetParams() => new()
    {
        new Erc721Attribute("LeftAmount", LeftAmount, DisplayType.Number)
    };

    public override string GetDescription() =>
        $"This NFT represents immediate access to {LeftAmount} units of the specified asset {PoolInfo.Token}.";
}