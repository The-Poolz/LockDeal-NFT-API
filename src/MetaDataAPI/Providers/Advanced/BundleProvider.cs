using System.Numerics;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Advanced;

public class BundleProvider : IProvider
{
    public byte ParametersCount => 1;

    public IEnumerable<Erc721Attribute> GetAttributes(params BigInteger[] values)
    {
        return AttributesService.GetProviderAttributes(values[0]);
    }
}