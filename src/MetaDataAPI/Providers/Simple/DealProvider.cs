using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Simple;

public class DealProvider : IProvider
{
    public byte ParametersCount => 1;
    public List<Erc721Attribute> Attributes { get; }

    public DealProvider(byte decimals, params BigInteger[] values)
    {
        var converter = new ConvertWei(decimals);
        Attributes = new List<Erc721Attribute>
        {
            new("LeftAmount", converter.WeiToEth(values[0]), DisplayType.Number)
        };
    }

    public string GetDescription(string token) =>
        $"This NFT represents immediate access to {Attributes[0].Value} units of the specified asset {token}.";
}