using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Simple;

public class TimedProvider : IProvider
{
    public ProviderName Name => ProviderName.Timed;
    public byte ParametersCount => 4;
    public List<Erc721Attribute> Attributes { get; }

    public TimedProvider(byte decimals, IReadOnlyList<BigInteger> values)
    {
        var converter = new ConvertWei(decimals);
        Attributes = new List<Erc721Attribute>
        {
            new("LeftAmount", converter.WeiToEth(values[0]), DisplayType.Number, converter.WeiToEth(values[3])),
            new("StartTime", values[1], DisplayType.Date),
            new("FinishTime", values[2], DisplayType.Date),
        };
        return attributes;
    }

    public string GetDescription(string token) =>
        $"This NFT governs a time-locked pool containing {attributes[0].Value} units of the asset {token}. Withdrawals are permitted in a linear fashion beginning at {attributes[1].Value}, culminating in full access at {attributes[2].Value}.";
}