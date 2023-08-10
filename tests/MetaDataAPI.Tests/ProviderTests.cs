using Xunit;
using FluentAssertions;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.Types;

namespace MetaDataAPI.Tests;

public class ProviderTests
{
    [Fact]
    public void Constructor_Should_Set_Name()
    {
        var rawAddress = new string('0', 24) + "2028C98AC1702E2bb934A3E88734ccaE42d44338".ToLower();

        var provider = new Provider(rawAddress);

        provider.Name.Should().Be(nameof(ProviderName.Deal));
    }

    [Fact]
    public void Constructor_Should_Throw_KeyNotFoundException_For_Unknown_Provider()
    {
        Action act = () => _ = new Provider(new string('0', 62));

        act.Should().Throw<KeyNotFoundException>().WithMessage("The given key '0x00000000000000000000000000000000000000' was not present in the dictionary.");
    }

    [Theory]
    [InlineData("0000000000000000000000002028c98ac1702e2bb934a3e88734ccae42d44338", typeof(Deal))]
    [InlineData("000000000000000000000000d5df3f41cc1df2cc42f3b683dd71ecc38913e0d6", typeof(Lock))]
    [InlineData("0000000000000000000000005c0cb6dd68102f51dc112c3cec1c7090d27853bc", typeof(Timed))]
    [InlineData("0000000000000000000000005eba5a16a42241d4e1d427c9ec1e4c0aec67e2a2", typeof(Refund))]
    [InlineData("000000000000000000000000f1ce27bd46f1f94ce8dc4de4c52d3d845efc29f0", typeof(Bundle))]
    [InlineData("000000000000000000000000db65ce03690e7044ac12f5e2ab640e7a355e9407", typeof(Collateral))]
    public void GetProvider_Should_Return_Expected_Provider_Type(string rawAddress, Type expectedType)
    {
        var provider = new Provider(rawAddress);

        var result = provider.GetProvider();

        result.GetType().Should().Be(expectedType);
    }

    [Fact]
    public void ParamsName_Should_Return_Expected_Params()
    {
        const string rawAddress = "0000000000000000000000002028c98ac1702e2bb934a3e88734ccae42d44338";
        var provider = new Provider(rawAddress);

        var result = provider.ParamsName;

        result.Should().BeEquivalentTo(new List<string> { "LeftAmount" });
    }
}