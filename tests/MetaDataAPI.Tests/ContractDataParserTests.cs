using Xunit;
using System.Numerics;
using FluentAssertions;

namespace MetaDataAPI.Tests;

public class ContractDataParserTests
{
    [Fact]
    public void ParseContractData_Should_Throw_Exception_For_Null_Or_Empty_Data()
    {
        Action act = () => ContractDataParser.ParseContractData("");

        act.Should().Throw<ArgumentNullException>().WithMessage("Invalid data. (Parameter 'rawData')");
    }

    [Fact]
    public void ParseContractData_ShouldParseCorrectly()
    {
        const string rawData = "0x0000000000000000000000002028c98ac1702e2bb934a3e88734ccae42d44338000000000000000000000000000000000000000000000000000000000000000000000000000000000000000057e0433551460e85dfc5a5ddaff4db199d0f960a00000000000000000000000066134461c865f824d294d8ca0d9080cc1acd05f600000000000000000000000000000000000000000000000000000000000000a000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000000";

        var result = ContractDataParser.ParseContractData(rawData);

        result.Provider.Should().BeEquivalentTo(new Provider("0000000000000000000000002028c98ac1702e2bb934a3e88734ccae42d44338"));
        result.PoolId.Should().Be(new BigInteger(0));
        result.Owner.Should().Be("0x57e0433551460e85dfc5a5ddaff4db199d0f960a");
        result.Token.Should().Be("0x66134461c865f824d294d8ca0d9080cc1acd05f6");
        result.Params.Should().BeEquivalentTo(new Dictionary<string, KeyValuePair<object, string>>
        {
            { "LeftAmount", new KeyValuePair<object, string>(new BigInteger(0), "number") }
        });
    }
}