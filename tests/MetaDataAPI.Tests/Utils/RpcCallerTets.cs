using Flurl.Http.Testing;
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
    }
}
