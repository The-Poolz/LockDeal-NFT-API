using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class CollateralProvider : Provider
{
    public override string ProviderName => nameof(CollateralProvider);
    public override string Description
    {
        get
        {
            var attributes = GetErc721Attributes().ToArray();

            return $"Exclusively utilized by project administrators, this NFT serves as a secure vault for holding refundable tokens. " +
                $"It holds {attributes[4].Value} for the main coin collector, {attributes[5].Value} for the token collector," +
                $" and {attributes[6].Value} for the main coin holder, valid until {attributes[1].Value}.";
        }
    }
    public override IEnumerable<Erc721Attribute> ProviderAttributes
    {
        get
        {
            var converter = new ConvertWei(PoolInfo.Token.Decimals);
            var result = new List<Erc721Attribute>
            {
                new("LeftAmount", converter.WeiToEth(PoolInfo.Params[0]), DisplayType.Number),
                new("FinishTime", PoolInfo.Params[1], DisplayType.Date),
                new("MainCoin",SubProvider[0].PoolInfo.Token.Address),
                new("Token", SubProvider[1].PoolInfo.Token.Address),
            };

            foreach (var provider in SubProvider)
            {
                result.AddRange(provider.GetErc721Attributes().Select(attribute =>
                    attribute.IncludeUnderscoreForTraitType(provider.PoolInfo.PoolId)));
            }
            return result;
        }
    }

    public Provider[] SubProvider { get; }
    
    public CollateralProvider(BasePoolInfo basePoolInfo)
        : base(basePoolInfo)
    {
        SubProvider = new Provider[3];

        for (var i = 0; i < 3; i++)
        {
            SubProvider[i] = basePoolInfo.Factory.Create(basePoolInfo.PoolId + i + 1);
        }
    }
}