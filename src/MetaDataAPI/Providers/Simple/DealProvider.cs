﻿using System.Numerics;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Simple;

public class DealProvider : IProvider
{
    public ProviderName Name => ProviderName.Deal;
    public BigInteger PoolId { get; }

    public DealProvider(BigInteger poolId)
    {
        PoolId = poolId;
    }

    public IEnumerable<Erc721Attribute> GetAttributes(params object[] values)
    {
        return new Erc721Attribute[]
        {
            new("LeftAmount", values[0], "number")
        };
    }
}