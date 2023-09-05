using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Simple;

public class DealProvider : IProvider
{
    public ProviderName Name => ProviderName.Deal;
    public byte ParametersCount => 1;
    public List<Erc721Attribute> Attributes { get; }

    public DealProvider(byte decimals, IReadOnlyList<BigInteger> values)
    {
        var converter = new ConvertWei(decimals);
        Attributes = new List<Erc721Attribute>
        {
            new("LeftAmount", converter.WeiToEth(values[0]), DisplayType.Number)
        };
        return attributes;
    }

    public string GetDescription(string token) =>
        $"This NFT represents immediate access to {attributes[0].Value} units of the specified asset {token}.";
}