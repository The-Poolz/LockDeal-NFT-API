using System.Numerics;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI;

public class BasePoolInfo
{
    public BigInteger PoolId { get; }
    public string Owner { get; }
    public string Token { get; }
    public IEnumerable<Erc721Attribute> Attributes { get; }

    public BasePoolInfo(IProvider provider, BigInteger poolId, string owner, string token, IReadOnlyCollection<BigInteger> parameters)
    {
        PoolId = poolId;
        Owner = owner;
        Token = token;

        if (provider.ParametersCount != parameters.Count)
        {
            throw new InvalidOperationException("Mismatch between keys and params counts");
        }

        Attributes = provider.GetAttributes(parameters.ToArray()).ToArray();
    }
}
