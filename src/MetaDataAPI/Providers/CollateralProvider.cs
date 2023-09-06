using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class CollateralProvider : IProvider
{
    public List<Erc721Attribute> Attributes { get; }
    public BasePoolInfo PoolInfo { get; }

    public CollateralProvider(BasePoolInfo basePoolInfo)
    {
        PoolInfo = basePoolInfo;
        var converter = new ConvertWei(basePoolInfo.Token.Decimals);
        Attributes = new List<Erc721Attribute>
        {
            new("LeftAmount", converter.WeiToEth(basePoolInfo.Params[0]), DisplayType.Number),
            new("FinishTime", basePoolInfo.Params[1], DisplayType.Date),
            AttributesService.GetMainCoinAttribute(basePoolInfo.PoolId),
            AttributesService.GetTokenAttribute(basePoolInfo.PoolId),
        };
        for (var id = basePoolInfo.PoolId + 1; id <= basePoolInfo.PoolId + 3; id++)
        {
            var providerAttributes = ProviderFactory.Create(id).Attributes;

            Attributes.AddRange(providerAttributes.Select(attribute =>
            attribute.IncludeUnderscoreForTraitType(id)));
        }
    }

    public string GetDescription() =>
        $"Exclusively utilized by project administrators, this NFT serves as a secure vault for holding refundable tokens. " +
        $"It holds {Attributes[4].Value} for the main coin collector, {Attributes[5].Value} for the token collector," +
        $" and {Attributes[6].Value} for the main coin holder, valid until {Attributes[1].Value}.";
}