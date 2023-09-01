using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;
using Org.BouncyCastle.Asn1.Cms;

namespace MetaDataAPI.Providers.Simple;

public class LockProvider : IProvider
{
    public byte ParametersCount => 2;
    public List<Erc721Attribute> Attributes { get; }

    public LockProvider(byte decimals, BigInteger[] values)
    {
        var converter = new ConvertWei(decimals);
        Attributes = new List<Erc721Attribute>
        {
            new("LeftAmount", converter.WeiToEth(values[0]), DisplayType.Number),
            new("StartTime", values[1], DisplayType.Date)
        };
    }
}