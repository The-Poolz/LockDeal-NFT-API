using Xunit;
using FluentAssertions;
using MetaDataAPI.Utils;

namespace MetaDataAPI.Tests.Utils;

public class MetadataParserTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_ShouldThrowArgumentNullException_WhenRawMetadataIsNullOrEmpty(string rawMetadata)
    {
        Action act = () => _ = new MetadataParser(rawMetadata);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Invalid data. (Parameter 'rawMetadata')");
    }

    [Fact]
    public void RemoveHexPrefix_ShouldReturnCorrectString_WhenHexStartsWith0x()
    {
        var hex = "0x" + new string('0', 64);

        var parser = new MetadataParser(hex);

        parser.GetProviderAddress().Should().Be("0x" + new string('0', 40));
    }
}