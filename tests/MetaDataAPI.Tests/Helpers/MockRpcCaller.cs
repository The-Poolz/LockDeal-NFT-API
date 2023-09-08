using MetaDataAPI.Utils;
using System.Numerics;

namespace MetaDataAPI.Tests.Helpers
{
    internal class MockRpcCaller : IRpcCaller
    {
        public MockRpcCaller()
        {
            metadata = StaticResults.MetaData;
            name = StaticResults.Names;
        }
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
        private readonly Dictionary<BigInteger,string> metadata;
        private readonly Dictionary<string, string> name;
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
    }
}
