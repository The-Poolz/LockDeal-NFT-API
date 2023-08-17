using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Simple;

public class DealProvider : IProvider
{
    private readonly string token;
    public byte ParametersCount => 1;

    public DealProvider(string token)
    {
        this.token = token;
    }

    public IEnumerable<Erc721Attribute> GetAttributes(params BigInteger[] values)
    {
        var decimals = RpcCaller.GetDecimals(token);
        var converter = new ConvertWei(decimals);
        return new Erc721Attribute[]
        {
            new("LeftAmount", converter.WeiToEth(values[0]), DisplayType.Number)
        };
    }
}