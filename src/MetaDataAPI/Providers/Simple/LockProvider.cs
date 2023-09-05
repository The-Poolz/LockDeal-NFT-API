using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Simple;

public class LockProvider : IProvider
{
    public ProviderName Name => ProviderName.Lock;
    public byte ParametersCount => 2;
    public List<Erc721Attribute> Attributes { get; }

    public LockProvider(byte decimals, IReadOnlyList<BigInteger> values)
    {
        var converter = new ConvertWei(decimals);
        Attributes = new List<Erc721Attribute>
        {
            new("LeftAmount", converter.WeiToEth(values[0]), DisplayType.Number),
            new("StartTime", values[1], DisplayType.Date)
        };
        return attributes;
    }

    public string GetDescription(string token) =>
        $"This NFT securely locks {attributes[0].Value} units of the asset {token}. Access to these assets will commence on the designated start time of {attributes[1].Value}.";
}