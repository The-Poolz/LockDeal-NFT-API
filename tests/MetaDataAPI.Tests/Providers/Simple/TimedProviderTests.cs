﻿using Xunit;
using System.Numerics;
using FluentAssertions;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Providers.Simple;

namespace MetaDataAPI.Tests.Providers.Simple;

public class TimedProviderTests
{
    [Fact]
    public void GetAttributes_ShouldReturnCorrectAttributes()
    {
        var provider = new TimedProvider();
        var values = new BigInteger[] { 42, 1692106619, 1692106620, 100 };
        var result = provider.GetAttributes(values).ToArray();

        provider.ParametersCount.Should().Be(4);
        result.Should().HaveCount(3);
        result.Should().BeEquivalentTo(new Erc721Attribute[]
        {
            new("LeftAmount", values[0], "number", values[3]),
            new("StartTime", values[1], "date"),
            new("FinishTime", values[2], "date")
        });
    }
}