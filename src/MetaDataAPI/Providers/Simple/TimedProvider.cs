using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Simple;

public class TimedProvider : IProvider
{
    public byte ParametersCount => 4;
    public List<Erc721Attribute> Attributes { get; }
    public BasePoolInfo PoolInfo { get; }

    public TimedProvider(BasePoolInfo basePoolInfo)
    {
        PoolInfo = basePoolInfo;
        var converter = new ConvertWei(basePoolInfo.Token.Decimals);
        Attributes = new List<Erc721Attribute>
        {
            new("LeftAmount", converter.WeiToEth(basePoolInfo.Params[0]), DisplayType.Number),
            new("StartAmount",converter.WeiToEth(basePoolInfo.Params[3]),DisplayType.Number),
            new("StartTime", basePoolInfo.Params[1], DisplayType.Date),
            new("FinishTime", basePoolInfo.Params[2], DisplayType.Date),
        };
    }

    public string GetDescription() =>
        $"This NFT governs a time-locked pool containing {Attributes[0].Value} units of the asset {PoolInfo.Token}. Withdrawals are permitted in a linear fashion beginning at {Attributes[1].Value}, culminating in full access at {Attributes[2].Value}.";
}