using MetaDataAPI.Utils;
using System.Numerics;

namespace MetaDataAPI.Tests.Helpers;

internal class MockRpcError : IRpcCaller
{
    public byte GetDecimals(string token)
    {
        throw new NotImplementedException();
    }

    public string GetMetadata(BigInteger poolId)
    {
        throw new NotImplementedException();
    }

    public string GetName(string address)
    {
        throw new NotImplementedException();
    }

    public string GetSymbol(string address)
    {
        throw new NotImplementedException();
    }

    public BigInteger GetTotalSupply(string address)
    {
        return 99;
    }
}
