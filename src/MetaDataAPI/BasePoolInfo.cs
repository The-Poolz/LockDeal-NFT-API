using System.Numerics;

namespace MetaDataAPI;

public class BasePoolInfo
{
    public Provider Provider { get; }
    public BigInteger PoolId { get; }
    public string Owner { get; }
    public string Token { get; }
    public Dictionary<string, BigInteger> Params { get; }

    public BasePoolInfo(Provider provider, BigInteger poolId, string owner, string token, IReadOnlyCollection<BigInteger> parameters)
    {
        Provider = provider;
        PoolId = poolId;
        Owner = owner;
        Token = token;

        if (Provider.ParamsNames.Count != parameters.Count)
        {
            throw new InvalidOperationException("Mismatch between keys and params counts");
        }

        Params = Provider.ParamsNames.Zip(parameters, (k, v) => new { Key = k, Value = v })
            .ToDictionary(x => x.Key, x => x.Value);
    }
}