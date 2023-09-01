using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;
using Org.BouncyCastle.Asn1.Cms;

namespace MetaDataAPI.Providers.Simple;

public class TimedProvider : IProvider
{
    public byte ParametersCount => 4;
    public List<Erc721Attribute> Attributes { get; }

    public TimedProvider(byte decimals, BigInteger[] values)
    {
        var converter = new ConvertWei(decimals);
        Attributes = new List<Erc721Attribute>
        {
            new("LeftAmount", converter.WeiToEth(values[0]), DisplayType.Number, converter.WeiToEth(values[3])),
            new("StartTime", values[1], DisplayType.Date),
            new("FinishTime", values[2], DisplayType.Date),
        };
    }
}