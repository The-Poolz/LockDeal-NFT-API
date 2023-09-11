using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class CollateralProvider : IProvider
{
    public List<Erc721Attribute> Attributes { get; }
    public BasePoolInfo PoolInfo { get; }
    public IProvider[] SubProvider { get; } = new IProvider[3];

    public CollateralProvider(BasePoolInfo basePoolInfo)
    {
        PoolInfo = basePoolInfo;

        for (var i = 0; i < 3; i++)
        {
            SubProvider[i] = basePoolInfo.Factory.Create(basePoolInfo.PoolId + i + 1);
        }

        var converter = new ConvertWei(basePoolInfo.Token.Decimals);
        Attributes = new List<Erc721Attribute>
        {
            new("LeftAmount", converter.WeiToEth(basePoolInfo.Params[0]), DisplayType.Number),
            new("FinishTime", basePoolInfo.Params[1], DisplayType.Date),
            new("MainCoin",SubProvider[0].PoolInfo.Token.Address),
            new("Token", SubProvider[1].PoolInfo.Token.Address),
        };

        foreach (var provider in SubProvider)
        {
            Attributes.AddRange(provider.Attributes.Select(attribute =>
            attribute.IncludeUnderscoreForTraitType(provider.PoolInfo.PoolId)));
        }
    }

    public string GetDescription() =>
        $"Exclusively utilized by project administrators, this NFT serves as a secure vault for holding refundable tokens. " +
        $"It holds {Attributes[4].Value} for the main coin collector, {Attributes[5].Value} for the token collector," +
        $" and {Attributes[6].Value} for the main coin holder, valid until {Attributes[1].Value}.";
}