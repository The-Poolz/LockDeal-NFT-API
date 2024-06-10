using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;
using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

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
                $"Exclusively utilized by project administrators, this NFT serves as a secure vault for holding refundable tokens {Token}, for Main Coin {MainCoin}. " +
                $"It holds {MainCoinCollectorAmount} for the main coin collector, {TokenCollectorAmount} for the token collector," +
                $" and {MainCoinHolderAmount} for the main coin holder, valid until {FinishTime}.";

    public Erc20Token MainCoin => new (PoolInfo.Token);
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
    public uint FinishTimestamp => (uint)PoolInfo.Params[1];
    internal DateTime FinishTime => TimeUtils.FromUnixTimestamp(FinishTimestamp);
    internal Dictionary<CollateralType,DealProvider> SubProvider { get; }

    public CollateralProvider(BasePoolInfo basePoolInfo) : base(new[] { basePoolInfo })
    {
        //This called when its called from refund - no need for the inner data
        SubProvider = new Dictionary<CollateralType, DealProvider>();
    }
    public CollateralProvider(BasePoolInfo[] basePoolInfo)
        : base(basePoolInfo)
    {
        SubProvider = Enum.GetValues(typeof(CollateralType))
                          .Cast<CollateralType>()
                          .ToDictionary(
                            val => val,
                            val => new DealProvider(basePoolInfo[(int)val]));
    }
}