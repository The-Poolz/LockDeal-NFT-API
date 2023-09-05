using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public interface IProvider
{
    public byte ParametersCount { get; }
    public string GetDescription(string token);
    public List<Erc721Attribute> Attributes { get; }
}
