using System.Numerics;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI;

public class BasePoolInfo
{
    public Provider Provider { get; }
    public BigInteger PoolId { get; }
    public string Owner { get; }
    public string Token { get; }
    public IEnumerable<Erc721Attribute> Attributes { get; }

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

        Attributes = Provider.ParamsNames.Zip(parameters, (pair, value) =>
            new Erc721Attribute(pair.Key, value, pair.Value));
    }
}