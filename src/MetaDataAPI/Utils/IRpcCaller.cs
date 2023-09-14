using System.Numerics;

namespace MetaDataAPI.Utils;

public interface IRpcCaller
{
    public string GetMetadata(BigInteger poolId);
    public string GetName(string address);
    public string GetSymbol(string address);
    public byte GetDecimals(string token);
    public BigInteger GetTotalSupply(string address);
}