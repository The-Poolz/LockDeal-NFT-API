using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Models;
using System.Numerics;
using MetaDataAPI.Models.DynamoDb;
using poolz.finance.csharp.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class RefundProvider : Provider
{
    public override string ProviderName => nameof(RefundProvider);
    public override string Description =>
        $"This NFT encompasses {LeftAmount} units of the asset {PoolInfo.Token} " +
        $"with an associated refund rate of {Rate}. Post rate calculation, the refundable " +
        $"amount in the primary asset {CollateralProvider.MainCoin} will be {MainCoinAmount}.";

    public Provider SubProvider { get; }
    public CollateralProvider CollateralProvider { get; }
    [Display(DisplayType.Number)]
    public decimal Rate => new ConvertWei(21).WeiToEth(PoolInfo.Params[2]);
    [Display(DisplayType.Number)]
    public decimal MainCoinAmount => SubProvider.LeftAmount * Rate;
    [Display(DisplayType.Number)]
    public BigInteger MainCoinCollection => CollateralProvider.MainCoinCollection;
    [Display(DisplayType.String)]
    public string SubProviderName => SubProvider.ProviderName;

    public override List<DynamoDbItem> DynamoDbAttributes
    {
        get
        {
            var dynamoDbAttributes = new List<DynamoDbItem>
            {
                new(ProviderName, PoolInfo, new List<Erc721Attribute>
                {
                    new("Rate", Rate),
                    new("MainCoinAmount", MainCoinAmount),
                    new("MainCoinCollection", MainCoinCollection)
                }),
                new(SubProvider.ProviderName, SubProvider.PoolInfo, SubProvider.Attributes.Where(attr => attr.TraitType != "ProviderName").ToList())
            };

            return dynamoDbAttributes;
        }
    }

    public RefundProvider(BasePoolInfo[] basePoolInfo)
        : base(basePoolInfo)
    {
        SubProvider = ProviderFactory.Create(new[] { basePoolInfo[1] });
        CollateralProvider = new CollateralProvider(basePoolInfo[2]);
    }
}