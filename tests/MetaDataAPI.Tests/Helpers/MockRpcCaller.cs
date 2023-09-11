using System.Numerics;
using MetaDataAPI.Utils;

namespace MetaDataAPI.Tests.Helpers;

internal class MockRpcCaller : IRpcCaller
{
    private readonly Dictionary<BigInteger,string> metadata;
    private readonly Dictionary<string, string> name;

    public MockRpcCaller()
    {
        metadata = StaticResults.MetaData;
        name = StaticResults.Names;
    }

    public byte GetDecimals(string token) => 18;

    public string GetMetadata(BigInteger poolId) => metadata[poolId];

    public string GetName(string address) => name[address];

    public string GetSymbol(string address) => "TST";
}