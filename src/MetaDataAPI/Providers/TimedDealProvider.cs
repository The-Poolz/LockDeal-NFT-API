﻿using Nethereum.Web3;
using System.Numerics;
using MetaDataAPI.Extensions;
using MetaDataAPI.Services.ChainsInfo;
using MetaDataAPI.Providers.Attributes;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class TimedDealProvider : LockDealProvider
{
    [Erc721MetadataItem("finish time", DisplayType.Date)]
    public BigInteger FinishTime { get; }

    public string String_FinishTime => FinishTime.DateTimeStringFormat();
    public string StringLabel_FinishTime => "Finish time";

    [Erc721MetadataItem("start amount", DisplayType.Number)]
    public decimal StartAmount { get; }

    public TimedDealProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IServiceProvider serviceProvider)
        : base(poolsInfo, chainInfo, serviceProvider)
    {
        FinishTime = PoolInfo.Params[2];
        StartAmount = Web3.Convert.FromWei(PoolInfo.Params[3], Erc20Token.Decimals);
    }

    protected override string DescriptionTemplate =>
        "This NFT governs a time-locked pool containing {{LeftAmount}}/{{StartAmount}} units of the asset {{Erc20Token}}." +
        " Withdrawals are permitted in a linear fashion beginning at {{String_StartTime}}, culminating in full access at {{String_FinishTime}}.";
}