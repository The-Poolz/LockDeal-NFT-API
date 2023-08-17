using System.Numerics;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Models.Types;

namespace MetaDataAPI.Providers.Simple;

public class LockProvider : IProvider
{
    public byte ParametersCount => 2;

    public IEnumerable<Erc721Attribute> GetAttributes(params BigInteger[] values)
    {
        return new Erc721Attribute[]
        {
            new("LeftAmount", values[0], DisplayType.Number),
            new("StartTime", values[1], DisplayType.Date)
        };
    }
}