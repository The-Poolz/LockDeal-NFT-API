using System.Numerics;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public interface IProvider
{
    public ProviderName Name { get; }
    public IEnumerable<Erc721Attribute> GetAttributes(params BigInteger[] values);
}
