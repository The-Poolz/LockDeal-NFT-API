using System.Numerics;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Models.Types;

namespace MetaDataAPI.Providers.Advanced;

public class BundleProvider : IProvider
{
    public byte ParametersCount => 1;
    public ProviderName Name => ProviderName.Bundle;
    public List<Erc721Attribute> Attributes { get; }

    public BundleProvider(BigInteger poolId, byte decimals, IReadOnlyList<BigInteger> values)
    {
        Attributes = new List<Erc721Attribute>();
        for (var id = poolId + 1; id <= values[0]; id++)
        {
            var providerAttributes = AttributesService.GetProviderAttributes(id, decimals);

            Attributes.AddRange(providerAttributes.Select(attribute => 
            attribute.IncludeUnderscoreForTraitType(id)));
        }
    }

    public string GetDescription(string token)
    {
        throw new NotImplementedException();
    }
}