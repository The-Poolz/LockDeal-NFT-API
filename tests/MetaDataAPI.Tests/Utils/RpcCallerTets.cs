using FluentAssertions;
using Flurl.Http.Testing;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Providers;
using MetaDataAPI.Storage;
using MetaDataAPI.Utils;
using Xunit;

namespace MetaDataAPI.Tests.Utils
{
    public class RpcCallerTets
    {
        private static readonly string url = "https://localhost:5050";
        readonly internal RpcCaller _rpcCaller;
        public RpcCallerTets()
        {
            Environment.SetEnvironmentVariable("RPC_URL", url);
            _rpcCaller = new RpcCaller();
        }
        [Fact]
        public void Test_Ctor()
        {
            Assert.NotNull(_rpcCaller);
            Assert.Equal(url, RpcCaller.rpcUrl);
        }
        [Fact]
        public void Test_GetDecimals()
        {
            HttpTest httpTest = new();
            httpTest.ForCallsTo(url)
                .RespondWithJson(new { result = "12"});

           var result = _rpcCaller.GetDecimals("0x0000000000000000000000000000000000000001");
           Assert.Equal(18, result);
        }
        [Fact]
        public void Test_GetDecimalsOnZero()
        {
            HttpTest httpTest = new();
            httpTest.ForCallsTo(url)
                .RespondWithJson(new { result = "12" });

            var result = _rpcCaller.GetDecimals("0x0000000000000000000000000000000000000000");
            Assert.Equal(0, result);
        }
        [Fact]
        public void Test_GetName()
        {
            HttpTest httpTest = new();
            httpTest.ForCallsTo(url)
                .RespondWithJson(new { result = "0x0000111168656c6c6f20776f726c64" });

            var result = _rpcCaller.GetName("0x0000000000000000000000000000000000000000");
            Assert.Equal("hello world", result);
        }
        [Fact]
        public void Test_GetSymbol()
        {
            HttpTest httpTest = new();
            httpTest.ForCallsTo(url)
                .RespondWithJson(new { result = "0x0000111168656c6c6f20776f726c64" });

            var result = _rpcCaller.GetSymbol("0x0000000000000000000000000000000000000000");
            Assert.Equal("hello world", result);
        }
        [Fact]
        public void Test_GetMetdata()
        {
            Environment.SetEnvironmentVariable("LOCK_DEAL_NFT_ADDRESS", "0x0000000000000000000000000000000000000000");
            HttpTest httpTest = new();
            httpTest.ForCallsTo(url)
                .RespondWithJson(new { result = "hello world" });

            var result = _rpcCaller.GetMetadata(11);
            Assert.Equal("hello world", result);
        }
        [Fact]
        public void TestErc20()
        {
            HttpTest httpTest = new();
            httpTest.ForCallsTo(url).WithRequestBody($"*{MethodSignatures.Decimals}*")
                .RespondWithJson(new { result = "12" });
            httpTest.ForCallsTo(url).WithRequestBody($"*{MethodSignatures.Name}*")
                .RespondWithJson(new { result = "0x0000111168656c6c6f20776f726c64" });
            httpTest.ForCallsTo(url).WithRequestBody($"*{MethodSignatures.Symbol}*")
                .RespondWithJson(new { result = "0x0000111168656c6c6f20776f726c64" });


            var token = new Erc20Token("0x0000000000000000000000000000000000000001");

            token.Should().NotBeNull();
            token.Name.Should().Be("hello world");
            token.Symbol.Should().Be("hello world");
            token.Decimals.Should().Be(18);
        }
        [Fact]
        public void TestThrowBasePoolInfo()
        {
            Assert.Throws<ArgumentNullException>(() => new BasePoolInfo(string.Empty, new ProviderFactory()));
        }
    }
}
