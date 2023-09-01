using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public interface IProvider
{
    public byte ParametersCount { get; }
    public List<Erc721Attribute> Attributes { get; }
    public string GetDescription(string token = "");
}
