using System.Text;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class BundleProvider : Provider
{
    public override List<Erc721Attribute> GetParams()
    {
        var result = new List<Erc721Attribute>();
        for (var id = PoolInfo.PoolId + 1; id <= PoolInfo.Params[1]; id++)
        {
            var subProvider = PoolInfo.Factory.Create(id);
            SubProviders.Add(subProvider);
            result.AddRange(subProvider.Attributes.Select(attribute =>
            attribute.IncludeUnderscoreForTraitType(id)));
        }
        return result;
    }
    public List<IProvider> SubProviders { get; }

    public BundleProvider(BasePoolInfo basePoolInfo) : base(basePoolInfo)
    {
        SubProviders = new List<IProvider>();
        AddAttributes("BundleProvider");
    }

    public override string GetDescription()
    {
        var descriptionBuilder = new StringBuilder()
            .AppendLine("This NFT orchestrates a series of sub-pools to enable sophisticated asset management strategies. The following are the inner pools under its governance:");

        return SubProviders.Aggregate(descriptionBuilder, (sb, item) =>
        sb.AppendLine($"- {item.PoolInfo}: {item.GetDescription()}"))
            !.ToString();
    }
}