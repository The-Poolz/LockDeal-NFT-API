using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Models;
using System.Numerics;
using MetaDataAPI.Models.DynamoDb;

namespace MetaDataAPI.Providers;

public class RefundProvider : Provider
{
    public override string ProviderName => nameof(RefundProvider);
    public override string Description =>
        $"This NFT encompasses {LeftAmount} units of the asset {PoolInfo.Token} " +
        $"with an associated refund rate of {CollateralProvider.Rate}. Post rate calculation, the refundable " +
        $"amount in the primary asset {CollateralProvider.MainCoin} will be {MainCoinAmount}.";

    public Provider SubProvider { get; }

    public CollateralProvider CollateralProvider { get; }

    [Display(DisplayType.Number)]
    public decimal MainCoinAmount => new ConvertWei(CollateralProvider.MainCoin.Decimals).WeiToEth(PoolInfo.Params[1]);

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
                    new("Rate", CollateralProvider.Rate),
                    new("MainCoinAmount", MainCoinAmount),
                    new("MainCoinCollection", MainCoinCollection)
                }),
                new(SubProvider.ProviderName, SubProvider.PoolInfo, SubProvider.Attributes.Where(attr => attr.TraitType != "ProviderName").ToList())
            };

            return dynamoDbAttributes;
        }
    }

    public RefundProvider(BasePoolInfo basePoolInfo)
        : base(basePoolInfo)
    {
        SubProvider = basePoolInfo.Factory.Create(PoolInfo.PoolId + 1);
        var collateralPoolId = basePoolInfo.Factory.GetCollateralId(basePoolInfo.ProviderAddress, basePoolInfo.PoolId);
        CollateralProvider = basePoolInfo.Factory.Create<CollateralProvider>(collateralPoolId);
    }
}