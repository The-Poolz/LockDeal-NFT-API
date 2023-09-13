using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public interface IProvider
{
    public BasePoolInfo PoolInfo { get; }
    public List<Erc721Attribute> Attributes { get; }
    public string GetDescription();
}
