using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Simple;

public class LockProvider : IProvider
{
    public byte ParametersCount => 2;
    public List<Erc721Attribute> Attributes { get; }
    public BasePoolInfo PoolInfo { get; }

    public LockProvider(BasePoolInfo basePoolInfo)
    {
        PoolInfo = basePoolInfo;
        var converter = new ConvertWei(basePoolInfo.Token.Decimals);
        Attributes = new List<Erc721Attribute>
        {
            new("LeftAmount", converter.WeiToEth(basePoolInfo.Params[0]), DisplayType.Number),
            new("StartTime", basePoolInfo.Params[1], DisplayType.Date)
        };
    }

    public string GetDescription() =>
        $"This NFT securely locks {Attributes[0].Value} units of the asset {PoolInfo.Token}. Access to these assets will commence on the designated start time of {Attributes[1].Value}.";
}