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
                new("MainCoin", SubProvider[0].PoolInfo.Token.Address),
                new("Token", SubProvider[1].PoolInfo.Token.Address),
            };

            const int MAIN_COIN_COLLECTOR = 0;
            const int TOKEN_HOLDER = 1;
            const int MAIN_COIN_HOLDER = 2;
            for (var id = 0; id < SubProvider.Length; id++)
            {
                var provider = SubProvider[id];

                if (id == MAIN_COIN_COLLECTOR)
                {
                    result.AddRange(new Erc721Attribute[]
                    {
                        new("Main Coin Collector Amount", provider.LeftAmount, DisplayType.Number),
                        new("MainCoin VaultId", provider.PoolInfo.VaultId, DisplayType.Number),
                        new("MainCoin Name", provider.PoolInfo.Token.Name)
                    });
                }
                if (id == TOKEN_HOLDER)
                {
                    result.AddRange(new Erc721Attribute[]
                    {
                        new("Token Holder Amount", provider.LeftAmount, DisplayType.Number),
                        new("Token VaultId", provider.PoolInfo.VaultId, DisplayType.Number),
                        new("TokenName", provider.PoolInfo.Token.Name)
                    });
                }
                if (id == MAIN_COIN_HOLDER)
                {
                    result.AddRange(new Erc721Attribute[]
                    {
                        new("Main Coin Holder Amount", provider.LeftAmount, DisplayType.Number)
                    });
                }
            }

            return result;
        }
    }

    public DealProvider[] SubProvider { get; }
    
    public CollateralProvider(BasePoolInfo basePoolInfo)
        : base(basePoolInfo)
    {
        SubProvider = new DealProvider[3];

        for (var i = 0; i < 3; i++)
        {
            SubProvider[i] = (DealProvider)basePoolInfo.Factory.Create(basePoolInfo.PoolId + i + 1);
        }
    }
}