﻿using Net.Urlify;
using System.Numerics;
using Net.Urlify.Attributes;
using MetaDataAPI.Models.Extension;
using EnvironmentManager.Extensions;
using MetaDataAPI.ImageGeneration.UrlifyModels.Types;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.ImageGeneration.UrlifyModels;

public class BaseUrlifyModel : Urlify
{
    [QueryStringProperty("name", false)]
    public string Name { get; set; }

    [QueryStringProperty("id", false)]
    public BigInteger Id { get; set; }

    [QueryStringProperty("tA", false)]
    public QueryStringToken Token { get; set; }

    public BaseUrlifyModel(BasePoolInfo poolInfo) : this(new PoolInfo(poolInfo)) { }

    public BaseUrlifyModel(PoolInfo poolInfo) : base((string)Environments.NFT_HTML_ENDPOINT.Get())
    {
        Name = poolInfo.Name;
        Id = poolInfo.PoolId;
        Token = new QueryStringToken(poolInfo.Erc20Token.Name, "Left Amount", poolInfo.LeftAmount);
    }
}