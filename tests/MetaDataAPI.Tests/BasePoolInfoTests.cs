using Xunit;
using System.Numerics;
using FluentAssertions;
using MetaDataAPI.Tests.Helpers;

namespace MetaDataAPI.Tests;

public class BasePoolInfoTests : SetEnvironments
{
    [Fact]
    public void Constructor_ShouldThrowInvalidOperationException_WhenParamsCountsMismatch()
    {
        var provider = new Provider("0000000000000000000000002028c98ac1702e2bb934a3e88734ccae42d44338");
        var poolId = new BigInteger(0);
        const string owner = "0x57e0433551460e85dfc5a5ddaff4db199d0f960a";
        const string token = "0x66134461c865f824d294d8ca0d9080cc1acd05f6";
        var parameters = new BigInteger[] { new(1), new(2) };

        Action act = () => _ = new BasePoolInfo(provider, poolId, owner, token, parameters);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Mismatch between keys and params counts");
    }
}