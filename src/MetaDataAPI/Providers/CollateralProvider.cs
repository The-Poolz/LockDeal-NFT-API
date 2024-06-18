﻿using MetaDataAPI.BlockchainManager.Models;
using MetaDataAPI.Erc20Manager;
using MetaDataAPI.Providers.Attributes;
using MetaDataAPI.Providers.Attributes.Models;
using Net.Urlify.Attributes;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;
using System.Numerics;
using MetaDataAPI.Extensions;

namespace MetaDataAPI.Providers;

internal enum CollateralType
{
    MainCoinCollector = 1,
    TokenCollector = 2,
    MainCoinHolder = 3
}

public class CollateralProvider : AbstractProvider
{
    public Erc20Token MainCoin { get; }

    public new decimal LeftAmount => base.LeftAmount;

    [Erc721Attribute("main coin collection", DisplayType.Number)]
    public new BigInteger VaultId => PoolInfo.VaultId;

    [Erc721Attribute("collection", DisplayType.Number)]
    public BigInteger Collection => SubProvider[CollateralType.TokenCollector].PoolInfo.VaultId;

    [Erc721Attribute("main coin collector amount", DisplayType.Number)]
    public decimal MainCoinCollectorAmount => SubProvider[CollateralType.MainCoinCollector].LeftAmount;

    [Erc721Attribute("token collector amount", DisplayType.Number)]
    public decimal TokenCollectorAmount => SubProvider[CollateralType.TokenCollector].LeftAmount;

    [Erc721Attribute("main coin holder amount", DisplayType.Number)]
    public decimal MainCoinHolderAmount => SubProvider[CollateralType.MainCoinHolder].LeftAmount;

    [Erc721Attribute("finish time", DisplayType.Date)]
    public BigInteger FinishTime { get; }

    [QueryStringProperty("Finish time", order: 1)]
    public string QueryString_FinishTime => FinishTime.DateTimeStringFormat();

    internal Dictionary<CollateralType, DealProvider> SubProvider { get; }

    public CollateralProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo)
        : this(poolsInfo, chainInfo, new Erc20Provider())
    { }

    public CollateralProvider(BasePoolInfo poolInfo, ChainInfo chainInfo, IErc20Provider erc20Provider)
        : this(new []{ poolInfo }, chainInfo, erc20Provider)
    {
        MainCoin = erc20Provider.GetErc20Token(chainInfo, poolInfo.Token);
    }

    public CollateralProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IErc20Provider erc20Provider)
        : base(poolsInfo, chainInfo, erc20Provider)
    {
        MainCoin = Erc20Token;
        FinishTime = PoolInfo.Params[1];

        SubProvider = Enum.GetValues(typeof(CollateralType))
            .Cast<CollateralType>()
            .ToDictionary(
                val => val,
                val => new DealProvider(poolsInfo[(int)val], chainInfo, erc20Provider)
            );
    }

    // TODO: I think something wrong in "tokens {{Erc20Token}}, for Main Coin {MainCoin}." part of description
    protected override string DescriptionTemplate =>
        "Exclusively utilized by project administrators, this NFT serves as a secure vault for holding refundable tokens {{Erc20Token}}, for Main Coin {MainCoin}. " +
        "It holds {{MainCoinCollectorAmount}} for the main coin collector, {{TokenCollectorAmount}} for the token collector," +
        " and {{MainCoinHolderAmount}} for the main coin holder, valid until {{QueryString_FinishTime}}.";
}