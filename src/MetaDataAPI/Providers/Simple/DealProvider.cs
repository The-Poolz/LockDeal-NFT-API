using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Simple;

public class DealProvider : IProvider
{
    private readonly byte decimals;
    public byte ParametersCount => 1;
    public ProviderName Name => ProviderName.Deal;

    public DealProvider(byte decimals)
    {
        this.decimals = decimals;
    }

    public IEnumerable<Erc721Attribute> GetAttributes(params BigInteger[] values)
    {
        var converter = new ConvertWei(decimals);
        return new Erc721Attribute[]
        {
            new("LeftAmount", converter.WeiToEth(values[0]), DisplayType.Number)
        };
    }
}