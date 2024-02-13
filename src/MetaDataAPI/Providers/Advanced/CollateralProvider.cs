using System.Numerics;
using MetaDataAPI.Models;
using MetaDataAPI.Models.DynamoDb;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Providers.PoolsInfo;
using MetaDataAPI.Providers.Simple;
using MetaDataAPI.RPC.Models;
using MetaDataAPI.Utils;

namespace MetaDataAPI.Providers.Advanced;

public class CollateralProvider : Provider
{
    public override string ProviderName => nameof(CollateralProvider);
    internal enum CollateralType
    {
        MainCoinCollector = 1,
        TokenCollector = 2,
        MainCoinHolder = 3
    }
    public override string Description =>
        $"Exclusively utilized by project administrators, this NFT serves as a secure vault for holding refundable tokens {PoolInfo.Token}, for Main Coin {MainCoin}. " +
        $"It holds {MainCoinCollectorAmount} for the main coin collector, {TokenCollectorAmount} for the token collector," +
        $" and {MainCoinHolderAmount} for the main coin holder, valid until {FinishTime}.";

    public ERC20Token MainCoin => PoolInfo.Token;

    [Display(DisplayType.Number)]
    public BigInteger MainCoinCollection => PoolInfo.VaultId;

    [Display(DisplayType.Number)]
    public override BigInteger Collection => SubProvider[CollateralType.TokenCollector].PoolInfo.VaultId;

    [Display(DisplayType.Ignore)]
    public override decimal LeftAmount => base.LeftAmount;

    [Display(DisplayType.Number)]
    public decimal MainCoinCollectorAmount => SubProvider[CollateralType.MainCoinCollector].LeftAmount;

    [Display(DisplayType.Number)]
    public decimal TokenCollectorAmount => SubProvider[CollateralType.TokenCollector].LeftAmount;

    [Display(DisplayType.Number)]
    public decimal MainCoinHolderAmount => SubProvider[CollateralType.MainCoinHolder].LeftAmount;

    [Display(DisplayType.Date)]
    public uint FinishTimestamp => PoolInfo.FinishTimestamp;

    [Display(DisplayType.Number)]
    public decimal Rate => new ConvertWei(21).WeiToEth(PoolInfo.Params[2]);

    internal DateTime FinishTime => TimeUtils.FromUnixTimestamp(FinishTimestamp);
    internal Dictionary<CollateralType, DealProvider> SubProvider { get; }

    public override List<DynamoDbItem> DynamoDbAttributes
    {
        get
        {
            var dynamoDbAttributes = new List<DynamoDbItem>
            {
                new(ProviderName, PoolInfo, new List<Erc721Attribute>
                {
                    new("Collection", Collection),
                    new("LeftAmount", LeftAmount)
                })
            };
            dynamoDbAttributes.AddRange(SubProvider.Select(subProvider => new DynamoDbItem(subProvider.Value.ProviderName, subProvider.Value.PoolInfo, subProvider.Value.Attributes.Where(attr => attr.TraitType != "ProviderName").ToList())));

            return dynamoDbAttributes;
        }
    }

    public CollateralPoolInfo PoolInfo { get; }

    public CollateralProvider(CollateralPoolInfo poolInfo, List<DealPoolInfo> subProvidersInfo)
        : base(poolInfo)
    {
        PoolInfo = poolInfo;
        var subProviders = subProvidersInfo
            .Select(x => new DealProvider(x))
            .ToList();

        SubProvider = Enum.GetValues(typeof(CollateralType)).Cast<CollateralType>()
            .Where(type => (int)type - 1 < subProviders.Count)
            .ToDictionary(
                type => type,
                type => subProviders[(int)type - 1]
            );

    }
}