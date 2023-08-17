﻿using Xunit;
using System.Numerics;
using FluentAssertions;
using Flurl.Http.Testing;
using MetaDataAPI.Storage;
using MetaDataAPI.Providers;
using MetaDataAPI.Tests.Helpers;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Providers.Simple;
using MetaDataAPI.Providers.Advanced;

namespace MetaDataAPI.Tests.Providers;

public class ProviderFactoryTests : SetEnvironments
{
    private readonly BigInteger poolId = new(0);

    [Fact]
    public void Create_ShouldCreateDealProvider_WhenAddressIsProvided()
    {
        var provider = ProviderFactory.Create(Environments.DealAddress, poolId);

        provider.Should().BeOfType<DealProvider>();
        provider.GetAttributes(new BigInteger(0))
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("LeftAmount", new BigInteger(0), "number")
            });
    }

    [Fact]
    public void Create_ShouldCreateLockProvider_WhenAddressIsProvided()
    {
        var provider = ProviderFactory.Create(Environments.LockAddress, poolId);

        provider.Should().BeOfType<LockProvider>();
        provider.GetAttributes(new BigInteger(0), new BigInteger(1692090665))
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("LeftAmount", new BigInteger(0), "number"),
                new("StartTime", new BigInteger(1692090665), "date")
            });
    }

    [Fact]
    public void Create_ShouldCreateTimedProvider_WhenAddressIsProvided()
    {
        var provider = ProviderFactory.Create(Environments.TimedAddress, poolId);

        provider.Should().BeOfType<TimedProvider>();
        provider.GetAttributes(new BigInteger(0), new BigInteger(1692090665), new BigInteger(1692090665), new BigInteger(100))
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("LeftAmount", new BigInteger(0), "number", new BigInteger(100)),
                new("StartTime", new BigInteger(1692090665), "date"),
                new("FinishTime", new BigInteger(1692090665), "date"),
            });
    }

    [Fact]
    public void Create_ShouldCreateBundleProvider_WhenAddressIsProvided()
    {
        using var httpTest = new HttpTest();
        httpTest.ForCallsTo(HttpMock.RpcUrl)
            .RespondWith(HttpMock.DealResponse);

        var provider = ProviderFactory.Create(Environments.BundleAddress, poolId);

        provider.Should().BeOfType<BundleProvider>();
        provider.GetAttributes(new BigInteger(2))
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("LeftAmount_1", new BigInteger(0), "number"),
                new("LeftAmount_2", new BigInteger(0), "number")
            });
    }
}