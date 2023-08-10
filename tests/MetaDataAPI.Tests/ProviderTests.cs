using Xunit;
using FluentAssertions;

namespace MetaDataAPI.Tests;

public class ProviderTests
{
    [Fact]
    public void ParamsNames_Should_Throw_KeyNotFoundException_For_Unknown_Provider()
    {
        var provider = new Provider(new string('0', 62));

        Action act = () => _ = provider.ParamsNames;

        act.Should().Throw<KeyNotFoundException>().WithMessage("The given key '0x00000000000000000000000000000000000000' was not present in the dictionary.");
    }

    [Theory]
    [InlineData("0000000000000000000000002028c98ac1702e2bb934a3e88734ccae42d44338", new[] { "LeftAmount" })]
    [InlineData("000000000000000000000000d5df3f41cc1df2cc42f3b683dd71ecc38913e0d6", new[] { "LeftAmount", "StartTime" })]
    [InlineData("0000000000000000000000005c0cb6dd68102f51dc112c3cec1c7090d27853bc", new[] { "LeftAmount", "StartTime", "FinishTime", "StartAmount" })]
    [InlineData("0000000000000000000000005eba5a16a42241d4e1d427c9ec1e4c0aec67e2a2", new[] { "CollateralId", "RateToWei" })]
    [InlineData("000000000000000000000000f1ce27bd46f1f94ce8dc4de4c52d3d845efc29f0", new[] { "LastSubPoolId" })]
    [InlineData("000000000000000000000000db65ce03690e7044ac12f5e2ab640e7a355e9407", new[] { "FinishTime" })]
    public void ParamsNames_Should_Return_Expected_Provider_Type(string rawAddress, IReadOnlyCollection<string> expectedCollection)
    {
        var provider = new Provider(rawAddress);

        var result = provider.ParamsNames;

        result.Should().BeEquivalentTo(expectedCollection);
    }
}