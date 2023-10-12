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
    public override string Description
    {
        get
        {
            return $"This NFT encompasses {LeftAmount} units of the asset {PoolInfo.Token} " +
                $"with an associated refund rate of {Rate}. Post rate calculation, the refundable " +
                $"amount in the primary asset {CollateralProvider.MainCoin} will be {MainCoinAmount}.";
        }
    }

    public Provider SubProvider { get; }
    public CollateralProvider CollateralProvider { get; }
    [Display(DisplayType.Number)]
    public decimal Rate => new ConvertWei(18).WeiToEth(PoolInfo.Params[2]);
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
                new(ProviderName, new List<Erc721Attribute>
                {
                    new("Rate", Rate),
                    new("MainCoinAmount", MainCoinAmount),
                    new("MainCoinCollection", MainCoinCollection)
                }),
                new(SubProvider.ProviderName, SubProvider.Attributes.Where(attr => attr.TraitType != "ProviderName").ToList())
            };

            return dynamoDbAttributes;
        }
    }

    public RefundProvider(BasePoolInfo basePoolInfo)
        : base(basePoolInfo)
    {
        SubProvider = basePoolInfo.Factory.Create(PoolInfo.PoolId + 1);
        CollateralProvider = basePoolInfo.Factory.Create<CollateralProvider>(basePoolInfo.Params[1]);
    }
}