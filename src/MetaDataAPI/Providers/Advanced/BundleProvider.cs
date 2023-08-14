using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Advanced;

public class BundleProvider : IProvider
{
    public ProviderName Name => ProviderName.Bundle;
    public IEnumerable<Erc721Attribute> GetAttributes(params object[] values)
    {
        return new Erc721Attribute[0];
    }
}