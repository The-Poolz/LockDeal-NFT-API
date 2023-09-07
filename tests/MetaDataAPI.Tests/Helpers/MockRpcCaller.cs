using AutoMapper.Configuration.Annotations;
using MetaDataAPI.Utils;
using System.Numerics;

namespace MetaDataAPI.Tests.Helpers
{
    internal class MockRpcCaller : IRpcCaller
    {
        public MockRpcCaller()
        {
            //decimals = new Dictionary<string, byte>();
            metadata = new Dictionary<BigInteger, string>();
            name = new Dictionary<string, string>();
            //symbol = new Dictionary<string, string>();
        }
        //public MockRpcCaller AddDecimals(string token, byte decimals)
        //{
        //    this.decimals.Add(token, decimals);
        //    return this;
        //}
        public MockRpcCaller AddMetadata(BigInteger poolId, string metadata)
        {
            this.metadata.Add(poolId, metadata);
            return this;
        }
        public MockRpcCaller AddName(string address, string name)
        {
            this.name.Add(address, name);
            return this;
        }
        //public MockRpcCaller AddSymbol(string address, string symbol)
        //{
        //    this.symbol.Add(address, symbol);
        //    return this;
        //}
        //private readonly Dictionary<string,byte> decimals;
        private readonly Dictionary<BigInteger,string> metadata;
        private readonly Dictionary<string, string> name;
        //private readonly Dictionary<string, string> symbol;
        public byte GetDecimals(string token)
        {
            return 18;
        }

        public string GetMetadata(BigInteger poolId)
        {
            return metadata[poolId];
        }

        public string GetName(string address)
        {
            return name[address];
        }

        public string GetSymbol(string address)
        {
            return "TST";
        }
        public static MockRpcCaller InstallFullTest()
        {
            var result = new MockRpcCaller();
            foreach (var item in StaticResults.MetaData)
            {
                result.AddMetadata(item.Key, item.Value);
            }
            foreach (var item in StaticResults.Names)
            {
                result.AddName(item.Key, item.Value);
            }
            return result;
        }
    }
}
