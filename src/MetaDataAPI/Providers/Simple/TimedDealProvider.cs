using MetaDataAPI.Utils;
using MetaDataAPI.Models;
using MetaDataAPI.Models.Types;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;
using MetaDataAPI.ImageGeneration.UrlifyModels.Simple;
using MetaDataAPI.ImageGeneration.UrlifyModels;

namespace MetaDataAPI.Providers;

public class TimedDealProvider : LockDealProvider
{
    public override string ToString() => $"governs a time-locked pool containing {LeftAmount}" +
        $" beginning at {StartDateTime}, culminating in full access at {FinishDateTime}";

    public override string ProviderName => nameof(TimedDealProvider);

    public override string Description =>
        $"This NFT governs a time-locked pool containing {LeftAmount}/{StartAmount} units of the asset {PoolInfo.Token}." +
        $" Withdrawals are permitted in a linear fashion beginning at {StartDateTime}, culminating in full access at {FinishDateTime}.";

    [Display(DisplayType.Number)]
    public decimal StartAmount { get; }

    [Display(DisplayType.Date)]
    public uint FinishTime { get; }

    public DateTime FinishDateTime => TimeUtils.FromUnixTimestamp(FinishTime);

    public override BaseUrlifyModel Urlify => new TimedDealUrlifyModel(PoolInfo);

    public TimedDealProvider(BasePoolInfo[] basePoolInfo)
        : base(basePoolInfo)
    {
        var converter = new ConvertWei(Token.Decimals);
        var thisInfo = basePoolInfo.FirstOrDefault()!;
        FinishTime = (uint)thisInfo.Params[2];
        StartAmount = converter.WeiToEth(thisInfo.Params[3]);
    }
}