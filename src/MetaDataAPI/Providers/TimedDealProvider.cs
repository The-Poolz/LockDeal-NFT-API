using Nethereum.Web3;
using System.Numerics;
using Net.Urlify.Attributes;
using MetaDataAPI.Extensions;
using MetaDataAPI.Erc20Manager;
using MetaDataAPI.Providers.Attributes;
using MetaDataAPI.BlockchainManager.Models;
using MetaDataAPI.Providers.Attributes.Models;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class TimedDealProvider : LockDealProvider
{
    [QueryStringProperty("Finish time", order: 1)]
    public string QueryString_FinishTime => FinishTime.DateTimeStringFormat();

    [Erc721Attribute("finish time", DisplayType.Date)]
    public BigInteger FinishTime { get; }

    [Erc721Attribute("start amount", DisplayType.Number)]
    public decimal StartAmount { get; }

    public TimedDealProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IErc20Provider erc20Provider)
        : base(poolsInfo, chainInfo, erc20Provider)
    {
        FinishTime = PoolInfo.Params[2];
        StartAmount = Web3.Convert.FromWei(PoolInfo.Params[3], Erc20Token.Decimals);
    }

    protected override string DescriptionTemplate =>
        "This NFT governs a time-locked pool containing {{LeftAmount}}/{{StartAmount}} units of the asset {{Erc20Token}}." +
        " Withdrawals are permitted in a linear fashion beginning at {{QueryString_StartTime}}, culminating in full access at {{QueryString_FinishTime}}.";
}