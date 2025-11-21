using Moq;
using Xunit;
using Nethereum.Web3;
using FluentAssertions;
using System.Reflection;
using Nethereum.JsonRpc.Client;
using MetaDataAPI.Services.Http;
using Poolz.Finance.CSharp.Http;

namespace MetaDataAPI.Tests.Services.Http;

public class Web3FactoryTests
{
    [Fact]
    internal void ShouldCreateWeb3ClientUsingHttpClientFromFactory()
    {
        const string rpcUrl = "https://example.test/rpc";
        using var httpClient = new HttpClient(new HttpClientHandler());

        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(factory => factory.Create(rpcUrl, null)).Returns(httpClient);

        var factory = new Web3Factory(httpClientFactoryMock.Object);

        var web3 = factory.Create(rpcUrl);

        httpClientFactoryMock.Verify(f => f.Create(rpcUrl, null), Times.Once);
        web3.Should().BeOfType<Web3>();
        web3.Client.Should().BeOfType<RpcClient>();

        var rpcClient = (RpcClient)web3.Client;
        var httpClientField = typeof(RpcClient).GetField("_httpClient", BindingFlags.Instance | BindingFlags.NonPublic);
        httpClientField.Should().NotBeNull();
        httpClientField!.GetValue(rpcClient).Should().BeSameAs(httpClient);
    }
}