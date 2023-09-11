using Xunit;
using FluentAssertions;
using MetaDataAPI.Utils;
using Flurl.Http.Testing;
using MetaDataAPI.Storage;
using MetaDataAPI.Tests.Helpers;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Tests.Utils;

public class RpcCallerTests : SetEnvironments
{
    private const string ZeroAddress = "0x0000000000000000000000000000000000000000";
    private readonly RpcCaller rpcCaller = new();

    [Fact]
    public void GetDecimals_NotZeroAddress_ShouldReturn18Decimals()
    {
        using var httpTest = new HttpTest();
        httpTest
            .ForCallsTo(HttpMock.RpcUrl)
            .RespondWithJson(new { result = "12"});

        var result = rpcCaller.GetDecimals("0x0000000000000000000000000000000000000001");

        Assert.Equal(18, result);
    }

    [Fact]
    public void GetDecimals_ZeroAddress_ShouldReturnZeroDecimals()
    {
        using var httpTest = new HttpTest();
        httpTest.ForCallsTo(HttpMock.RpcUrl)
            .RespondWithJson(new { result = "12" });

        var result = rpcCaller.GetDecimals(ZeroAddress);

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetName_ShouldReturnExpectedName()
    {
        using var httpTest = new HttpTest();
        httpTest
            .ForCallsTo(HttpMock.RpcUrl)
            .RespondWithJson(new { result = "0x0000111168656c6c6f20776f726c64" });

        var result = rpcCaller.GetName(ZeroAddress);

        Assert.Equal("hello world", result);
    }

    [Fact]
    public void GetSymbol_ShouldReturnExpectedSymbol()
    {
        using var httpTest = new HttpTest();
        httpTest
            .ForCallsTo(HttpMock.RpcUrl)
            .RespondWithJson(new { result = "0x0000111168656c6c6f20776f726c64" });

        var result = rpcCaller.GetSymbol(ZeroAddress);

        Assert.Equal("hello world", result);
    }

    [Fact]
    public void GetMetadata_ShouldReturnExpectedMetadata()
    {
        using var httpTest = new HttpTest();
        httpTest
            .ForCallsTo(HttpMock.RpcUrl)
            .RespondWithJson(new { result = "hello world" });

        var result = rpcCaller.GetMetadata(11);

        Assert.Equal("hello world", result);
    }

    [Fact]
    public void Ctor_NotZeroAddress_ShouldReturnExpectedValuesInProps()
    {
        using var httpTest = new HttpTest();
        httpTest
            .ForCallsTo(HttpMock.RpcUrl)
            .WithRequestBody($"*{MethodSignatures.Decimals}*")
            .RespondWithJson(new { result = "12" });
        httpTest
            .ForCallsTo(HttpMock.RpcUrl)
            .WithRequestBody($"*{MethodSignatures.Name}*")
            .RespondWithJson(new { result = "0x0000111168656c6c6f20776f726c64" });
        httpTest
            .ForCallsTo(HttpMock.RpcUrl)
            .WithRequestBody($"*{MethodSignatures.Symbol}*")
            .RespondWithJson(new { result = "0x0000111168656c6c6f20776f726c64" });

        var result = new Erc20Token("0x0000000000000000000000000000000000000001");

        result.Should().NotBeNull();
        result.Name.Should().Be("hello world");
        result.Symbol.Should().Be("hello world");
        result.Decimals.Should().Be(18);
    }
}