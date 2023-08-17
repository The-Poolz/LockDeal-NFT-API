using System.Numerics;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Advanced;

public class CollateralProvider : IProvider
{
    private readonly BigInteger poolId;
    public byte ParametersCount => 2;

    public CollateralProvider(BigInteger poolId)
    {
        this.poolId = poolId;
    }

    public IEnumerable<Erc721Attribute> GetAttributes(params BigInteger[] values)
    {
        var attributes = new List<Erc721Attribute>
        {
            new("LeftAmount", values[0], DisplayType.Number),
            new("FinishTime", values[1], DisplayType.Date),
            AttributesService.GetMainCoinAttribute(poolId),
            AttributesService.GetTokenAttribute(poolId),
        };

        for (var id = poolId + 1; id <= poolId + 3; id++)
        {
            var providerAttributes = AttributesService.GetProviderAttributes(id);
            attributes.AddRange(providerAttributes.Select(attribute => attribute.IncludeUnderscoreForTraitType(id)));
        }

        return attributes;
    }
}