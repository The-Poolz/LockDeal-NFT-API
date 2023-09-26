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
                var attributes = provider.GetErc721Attributes();
                var modifiedAttributes = new List<Erc721Attribute>();

                foreach (var attribute in attributes)
                {
                    var newTraitType = attribute.TraitType;
                    switch (id)
                    {
                        case MAIN_COIN_COLLECTOR when newTraitType.Contains("TokenName"):
                            newTraitType = "MainCoin Name";
                            break;
                        case MAIN_COIN_COLLECTOR when newTraitType.Contains("LeftAmount"):
                            newTraitType = "Main Coin Collector Amount";
                            break;
                        case MAIN_COIN_COLLECTOR when newTraitType.Contains("VaultId"):
                            newTraitType = "MainCoin VaultId";
                            break;
                        case TOKEN_HOLDER when newTraitType.Contains("TokenName"):
                            continue;
                        case TOKEN_HOLDER when newTraitType.Contains("LeftAmount"):
                            newTraitType = "Token Holder Amount";
                            break;
                        case TOKEN_HOLDER when newTraitType.Contains("VaultId"):
                            newTraitType = "Token VaultId";
                            break;
                        case MAIN_COIN_HOLDER when newTraitType.Contains("TokenName") || newTraitType.Contains("VaultId"):
                            continue;
                        case MAIN_COIN_HOLDER when newTraitType.Contains("LeftAmount"):
                            newTraitType = "Main Coin Holder Amount";
                            break;
                    }

                    if (newTraitType.Contains("ProviderName"))
                    {
                        continue;
                    }

                    Enum.TryParse(attribute.DisplayType, true, out DisplayType displayType);
                    modifiedAttributes.Add(new Erc721Attribute(newTraitType, attribute.Value, displayType));
                }

                result.AddRange(modifiedAttributes);
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