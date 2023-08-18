﻿using Xunit;
using System.Numerics;
using FluentAssertions;
using Flurl.Http.Testing;
using MetaDataAPI.Storage;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Tests.Helpers;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Providers.Advanced;

namespace MetaDataAPI.Tests.Providers;

public class ProviderFactoryTests : SetEnvironments
{
    [Fact]
    public void Create_ShouldCreateBundleProvider_WhenAddressIsProvided()
    {
        using var httpTest = new HttpTest();
        httpTest.ForCallsTo(HttpMock.RpcUrl)
            .RespondWith(HttpMock.DealResponse);

        var provider = ProviderFactory.Create(Environments.BundleAddress, new BigInteger(0), HttpMock.TokenAddress);

        provider.Should().BeOfType<BundleProvider>();
        provider.GetAttributes(new BigInteger(2))
            .Should().BeEquivalentTo(new Erc721Attribute[]
            {
                new("LeftAmount_1", 0m, DisplayType.Number),
                new("LeftAmount_2", 0m, DisplayType.Number)
            });
    }
}