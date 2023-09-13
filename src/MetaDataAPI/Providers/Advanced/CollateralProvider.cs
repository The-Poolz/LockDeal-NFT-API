using Newtonsoft.Json;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class CollateralProvider : Provider
{
    [JsonIgnore]
    public IProvider[] SubProvider { get; } = new IProvider[3];
    
    public CollateralProvider(BasePoolInfo basePoolInfo) : base(basePoolInfo)
    {
        for (var i = 0; i < 3; i++)
        {
            SubProvider[i] = basePoolInfo.Factory.Create(basePoolInfo.PoolId + i + 1);
        }
        AddAttributes(nameof(CollateralProvider));
    }

    public override List<Erc721Attribute> GetParams()
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
            result.AddRange(provider.Attributes.Select(attribute =>
                attribute.IncludeUnderscoreForTraitType(provider.PoolInfo.PoolId)));
        }
        return result;
    }

    public override string GetDescription() =>
        $"Exclusively utilized by project administrators, this NFT serves as a secure vault for holding refundable tokens. " +
        $"It holds {Attributes[4].Value} for the main coin collector, {Attributes[5].Value} for the token collector," +
        $" and {Attributes[6].Value} for the main coin holder, valid until {Attributes[1].Value}.";
}