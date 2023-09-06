using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Advanced;

public class CollateralProvider : IProvider
{
    public byte ParametersCount => 2;
    public List<Erc721Attribute> Attributes { get; }

    public CollateralProvider(BigInteger poolId, byte decimals, IReadOnlyList<BigInteger> values)
    {
        var converter = new ConvertWei(decimals);
        Attributes = new List<Erc721Attribute>
        {
            new("LeftAmount", converter.WeiToEth(values[0]), DisplayType.Number),
            new("FinishTime", values[1], DisplayType.Date),
            AttributesService.GetMainCoinAttribute(poolId),
            AttributesService.GetTokenAttribute(poolId),
        };
        for (var id = poolId + 1; id <= poolId + 3; id++)
        {
            var providerAttributes = AttributesService.GetProviderAttributes(id);

            Attributes.AddRange(providerAttributes.Select(attribute =>
            attribute.IncludeUnderscoreForTraitType(id)));
        }
    }

    public string GetDescription(string token) =>
        $"Exclusively utilized by project administrators, this NFT serves as a secure vault for holding refundable tokens. " +
        $"It holds {Attributes[4].Value} for the main coin collector, {Attributes[5].Value} for the token collector," +
        $" and {Attributes[6].Value} for the main coin holder, valid until {Attributes[1].Value}.";
}