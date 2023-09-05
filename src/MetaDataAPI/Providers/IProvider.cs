using System.Numerics;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public interface IProvider
{
    public ProviderName Name { get; }
    public byte ParametersCount { get; }
    public string GetDescription(string token);
    public List<Erc721Attribute> Attributes { get; }
}
