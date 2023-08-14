using System.Numerics;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public interface IProvider
{
    public ProviderName Name { get; }
    public BigInteger PoolId { get; }
    public string ProviderAddress => Provider.ProvidersAddresses[Name];
    public IEnumerable<Erc721Attribute> GetAttributes(params object[] values);
}
