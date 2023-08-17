﻿using Xunit;
using System.Numerics;
using FluentAssertions;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Providers.Simple;

namespace MetaDataAPI.Tests.Providers.Simple;

public class LockProviderTests
{
    [Fact]
    public void GetAttributes_ShouldReturnCorrectAttributes()
    {
        var provider = new LockProvider();
        var values = new BigInteger[] { 42, 1692106619 };
        var result = provider.GetAttributes(values).ToArray();

        provider.ParametersCount.Should().Be(2);
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(new Erc721Attribute[]
        {
            new("LeftAmount", values[0], DisplayType.Number),
            new("StartTime", values[1], DisplayType.Date)
        });
    }
}