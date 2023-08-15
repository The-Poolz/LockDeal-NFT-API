using System.Numerics;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Simple;

public class LockProvider : IProvider
{
    public ProviderName Name => ProviderName.Lock;

    public IEnumerable<Erc721Attribute> GetAttributes(params BigInteger[] values)
    {
        return new Erc721Attribute[]
        {
            new("LeftAmount", values[0], "number"),
            new("StartTime", values[1], "date")
        };
    }
}