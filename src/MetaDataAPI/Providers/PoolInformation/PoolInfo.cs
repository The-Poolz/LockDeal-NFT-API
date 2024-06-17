﻿using System.Numerics;
using Nethereum.Util;
using Nethereum.Web3;
using MetaDataAPI.Erc20Manager;
using MetaDataAPI.Providers.Attributes;
using MetaDataAPI.Providers.Image.Models;
using MetaDataAPI.Providers.Attributes.Models;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

// ReSharper disable VirtualMemberCallInConstructor

namespace MetaDataAPI.Providers.PoolInformation;

public abstract class PoolInfo : BasePoolInfo
{
    public Erc20Token Erc20Token { get; }

    [Erc721Attribute("provider name", DisplayType.String)]
    public override string Name { get; set; }

    [Erc721Attribute("collection", DisplayType.Number)]
    public override BigInteger PoolId { get; set; }

    [Erc721Attribute("left amount", DisplayType.Number)]
    public BigDecimal LeftAmount { get; }

    public abstract string DescriptionTemplate { get; }

    public abstract UrlifyModelCreation UrlifyModelCreation { get; }

    protected PoolInfo(BasePoolInfo[] poolsInfo, Erc20Token erc20)
        : this(poolsInfo[0], erc20)
    { }

    private PoolInfo(BasePoolInfo poolInfo, Erc20Token erc20)
    {
        Provider = poolInfo.Provider;
        Name = poolInfo.Name;
        PoolId = poolInfo.PoolId;
        VaultId = poolInfo.VaultId;
        Owner = poolInfo.Owner;
        Token = poolInfo.Token;
        Params = poolInfo.Params;

        Erc20Token = erc20;
        LeftAmount = Web3.Convert.FromWeiToBigDecimal(Params[0], Erc20Token.Decimals);
    }
}