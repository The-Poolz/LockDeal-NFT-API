using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Simple;

public class TimedProvider : IProvider
{
    private readonly byte decimals;
    private List<Erc721Attribute> attributes = new();
    public byte ParametersCount => 4;
    public ProviderName Name => ProviderName.Timed;

    public TimedProvider(byte decimals)
    {
        this.decimals = decimals;
    }

    public IEnumerable<Erc721Attribute> GetAttributes(params BigInteger[] values)
    {
        var converter = new ConvertWei(decimals);
        attributes = new List<Erc721Attribute>
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