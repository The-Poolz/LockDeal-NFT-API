using System.Numerics;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Utils;

namespace MetaDataAPI.Tests.Helpers;

internal class MockRpcCaller : IRpcCaller
{
    private readonly Dictionary<BigInteger,string> metadata;
    private readonly Dictionary<string, string> name;
    private readonly bool error;

    public MockRpcCaller(bool error = false)
    {
        this.error = error;
        metadata = StaticResults.MetaData;
        name = StaticResults.Names;
    }

    public byte GetDecimals(string token) => 18;

    public List<BasePoolInfo> GetFullData(BigInteger poolId)
    {
        throw new NotImplementedException();
    }

    public string GetMetadata(BigInteger poolId) => metadata[poolId];

    public string GetName(string address) => name[address];

    public string GetSymbol(string address) => "TST";

    public BigInteger GetTotalSupply(string address) => error ? throw new Exception() : 100000;
}