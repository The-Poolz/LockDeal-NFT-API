﻿using System.Numerics;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Simple;

public class TimedProvider : IProvider
{
    public byte ParametersCount => 4;

    public IEnumerable<Erc721Attribute> GetAttributes(params BigInteger[] values)
    {
        return new Erc721Attribute[]
        {
            new("LeftAmount", values[0], "number", values[3]),
            new("StartTime", values[1], "date"),
            new("FinishTime", values[2], "date"),
        };
    }
}