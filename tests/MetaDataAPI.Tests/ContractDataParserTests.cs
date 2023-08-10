using Xunit;
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
}