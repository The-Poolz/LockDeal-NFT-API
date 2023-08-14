using System.Numerics;

namespace MetaDataAPI.Providers;

public interface IAdvancedProvider : IProvider
{
    public BigInteger PoolId { get; }
}