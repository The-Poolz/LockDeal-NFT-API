using System.Numerics;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Models.Types;

namespace MetaDataAPI.Providers.Advanced;

public class BundleProvider : IProvider
{
    private readonly byte decimals;
    private readonly BigInteger poolId;
    public byte ParametersCount => 1;
    public ProviderName Name => ProviderName.Bundle;

    public BundleProvider(BigInteger poolId, byte decimals)
    {
        this.poolId = poolId;
        this.decimals = decimals;
    }

    public IEnumerable<Erc721Attribute> GetAttributes(params BigInteger[] values)
    {
        var attributes = new List<Erc721Attribute>();

        for (var id = poolId + 1; id <= values[0]; id++)
        {
            var providerAttributes = AttributesService.GetProviderAttributes(id, decimals);
            attributes.AddRange(providerAttributes.Select(attribute => attribute.IncludeUnderscoreForTraitType(id)));
        }

        return attributes;
    }

    public string GetDescription(string token)
    {
        throw new NotImplementedException();
    }
}