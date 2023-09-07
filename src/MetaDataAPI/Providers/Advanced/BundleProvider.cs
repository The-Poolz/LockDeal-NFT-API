using System.Text;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class BundleProvider : IProvider
{
    public List<Erc721Attribute> Attributes { get; }
    public BasePoolInfo PoolInfo { get; }
    public List<IProvider> SubProviders { get; }
    public BundleProvider(BasePoolInfo basePoolInfo)
    {
        PoolInfo = basePoolInfo;
        var lastSubPoolId = basePoolInfo.Params[0];
        Attributes = new List<Erc721Attribute>();
        SubProviders = new List<IProvider>();
        for (var id = PoolInfo.PoolId + 1; id <= lastSubPoolId; id++)
        {
            var subProvider = ProviderFactory.FromPoolId(id);
            SubProviders.Add(subProvider);
            Attributes.AddRange(subProvider.Attributes.Select(attribute =>
            attribute.IncludeUnderscoreForTraitType(id)));
        }
    }

    public string GetDescription()
    {
        var descriptionBuilder = new StringBuilder()
            .AppendLine("This NFT orchestrates a series of sub-pools to enable sophisticated asset management strategies. The following are the inner pools under its governance:");

        return SubProviders.Aggregate(descriptionBuilder, (sb, item) =>
        sb.AppendLine($"- {item.PoolInfo}: {item.GetDescription()}"))
            !.ToString();
    }

}