using MetaDataAPI.Providers.AttributesProviders.Models;

namespace MetaDataAPI.Providers.AttributesProviders;

public interface IAttributesProvider
{
    public IEnumerable<Erc721Attribute> Attributes();
}