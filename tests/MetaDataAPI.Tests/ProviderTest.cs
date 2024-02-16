using Xunit;
using MetaDataAPI.Providers;
using MetaDataAPI.Tests.Helpers;

namespace MetaDataAPI.Tests;

public class ProviderFactoryTests : SetEnvironments
{
    public static IEnumerable<object[]> ProviderTestData()
    {
        yield return new object[] { 8786, "DealProvider" };
        yield return new object[] { 8787, "LockDealProvider" };
        yield return new object[] { 8788, "TimedDealProvider" };
        yield return new object[] { 8789, "RefundProvider" };
        yield return new object[] { 8791, "CollateralProvider" };
        yield return new object[] { 8796, "DelayVaultProvider" };
    }

    [Theory]
    [MemberData(nameof(ProviderTestData))]
    public void ProviderTest(int providerId, string expectedProviderName)
    {
        var factory = new ProviderFactory();
        var result = factory.Create(new(providerId));

        Assert.NotNull(result);
        Assert.Equal(expectedProviderName, result.ProviderName);
    }
}
