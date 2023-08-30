using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;
using Nethereum.Contracts.Standards.ERC20.TokenList;

namespace MetaDataAPI.Providers.Simple;

public class DealProvider : IProvider
{
    private readonly byte decimals;
    private List<Erc721Attribute> attributes = new();
    public byte ParametersCount => 1;
    public ProviderName Name => ProviderName.Deal;

    public DealProvider(byte decimals)
    {
        this.decimals = decimals;
    }

    public IEnumerable<Erc721Attribute> GetAttributes(params BigInteger[] values)
    {
        var converter = new ConvertWei(decimals);
        attributes = new List<Erc721Attribute>
        {
            new("LeftAmount", converter.WeiToEth(values[0]), DisplayType.Number)
        };
        return attributes;
    }

    public string DealDescription(object leftAmonut, string token) =>
        $"This NFT represents immediate access to {leftAmonut} units of the specified asset {token}.";

    public string GetDescription(string token) =>
        DealDescription(attributes[0].Value, token);
}