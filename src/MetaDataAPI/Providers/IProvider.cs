using System.Numerics;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public interface IProvider
{
    public byte ParametersCount { get; }
    public IEnumerable<Erc721Attribute> GetAttributes(params BigInteger[] values);
}
