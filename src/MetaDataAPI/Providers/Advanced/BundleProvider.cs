using System.Text;
using System.Numerics;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class BundleProvider : IProvider
{
    private readonly BigInteger lastSubPoolId;
    public List<Erc721Attribute> Attributes { get; }
    public BasePoolInfo PoolInfo { get; }
    public List<IProvider> SubProviders { get; }
    public BundleProvider(BasePoolInfo basePoolInfo)
    {
        PoolInfo = basePoolInfo;
        lastSubPoolId = basePoolInfo.Params[0];
        Attributes = new List<Erc721Attribute>();
        SubProviders = new List<IProvider>();
        for (var id = PoolInfo.PoolId + 1; id <= lastSubPoolId; id++)
        {
            var subProvider = ProviderFactory.Create(id);
            SubProviders.Add(subProvider);
            Attributes.AddRange(subProvider.Attributes.Select(attribute =>
            attribute.IncludeUnderscoreForTraitType(id)));
        }
    }

    public string GetDescription()
    {
        var descriptionBuilder = new StringBuilder().AppendLine("This NFT orchestrates a series of sub-pools to enable sophisticated asset management strategies. The following are the inner pools under its governance:");

        foreach (var item in SubProviders)
        {
            descriptionBuilder.AppendLine($"- {item.PoolInfo}: {item.GetDescription()}");
        }

        return descriptionBuilder.ToString();
    }
}