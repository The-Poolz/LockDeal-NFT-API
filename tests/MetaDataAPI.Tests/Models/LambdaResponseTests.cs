using Xunit;
using FluentAssertions;
using MetaDataAPI.Models;
using MetaDataAPI.Models.Errors;

namespace MetaDataAPI.Tests.Models;

public class LambdaResponseTests
{
    [Fact]
    public void OptionsResponse_ShouldIncludeCorsHeaders()
    {
        var response = new OptionsResponse();

        response.Headers.Should().ContainKey("Access-Control-Allow-Origin").WhoseValue.Should().Be("*");
        response.Headers.Should().ContainKey("Access-Control-Allow-Headers").WhoseValue.Should().Be("Content-Type");
        response.Headers.Should().ContainKey("Access-Control-Allow-Methods").WhoseValue.Should().Be("GET,OPTIONS");
    }

    [Fact]
    public void ErrorResponse_ShouldIncludeCorsHeaders()
    {
        var response = new GeneralErrorResponse();

        response.Headers.Should().ContainKey("Access-Control-Allow-Origin").WhoseValue.Should().Be("*");
        response.Headers.Should().ContainKey("Access-Control-Allow-Headers").WhoseValue.Should().Be("Content-Type");
        response.Headers.Should().ContainKey("Access-Control-Allow-Methods").WhoseValue.Should().Be("GET,OPTIONS");
    }
}