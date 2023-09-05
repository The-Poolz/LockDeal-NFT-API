﻿using System.Numerics;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI;

public class BasePoolInfo
{
    public IProvider Provider { get; }
    public BigInteger PoolId { get; }
    public string Owner { get; }
    public string Token { get; }
    public IEnumerable<Erc721Attribute> Attributes { get; }

    public BasePoolInfo(IProvider provider, BigInteger poolId, string owner, string token)
    {
        Provider = provider;
        PoolId = poolId;
        Owner = owner;
        Token = token;

        Attributes = provider.Attributes;
    }
}
