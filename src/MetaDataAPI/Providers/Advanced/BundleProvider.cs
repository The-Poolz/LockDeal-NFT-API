using System.Numerics;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Advanced;

public class BundleProvider : IProvider
{
    private readonly BigInteger poolId;
    public byte ParametersCount => 1;
    public List<Erc721Attribute> Attributes { get; }

    public BundleProvider(BigInteger poolId, byte decimals, BigInteger[] values)
    {
        this.poolId = poolId;
        Attributes = new();
        for (var id = poolId + 1; id <= values[0]; id++)
        {
            var providerAttributes = AttributesService.GetProviderAttributes(id, decimals);

            Attributes.AddRange(providerAttributes.Select(attribute => 
            attribute.IncludeUnderscoreForTraitType(id)));
        }
    }

}