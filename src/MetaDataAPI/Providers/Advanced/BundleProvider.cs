using System.Text;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class BundleProvider : Provider
{
    public override string ProviderName => nameof(BundleProvider);
    public override string Description
    {
        get
        {
            var descriptionBuilder = new StringBuilder()
                .AppendLine("This NFT orchestrates a series of sub-pools to enable sophisticated asset management strategies. The following are the inner pools under its governance:");

            return SubProviders.Aggregate(descriptionBuilder, (sb, item) =>
                sb.AppendLine($"- {item.PoolInfo}: {item.Description}")).ToString();
        }
    }

    public override IEnumerable<Erc721Attribute> ProviderAttributes
    {
        get
        {
            var isAddedVaultId = false;
            var result = new List<Erc721Attribute>();
            for (var poolId = PoolInfo.PoolId + 1; poolId <= PoolInfo.Params[1]; poolId++)
            {
                var subProvider = PoolInfo.Factory.Create(poolId);
                SubProviders.Add(subProvider);

                if (!isAddedVaultId)
                {
                    result.AddRange(subProvider.ProviderAttributes.Where(x => x.TraitType.Contains("VaultId")));

                    isAddedVaultId = true;
                }

                result.AddRange(subProvider.ProviderAttributes.Where(x => !x.TraitType.Contains("VaultId")).Select(
                    attribute => attribute.IncludeUnderscoreForTraitType(poolId)));
            }
            return result;
        }
    }

    public List<Provider> SubProviders { get; }

    public BundleProvider(BasePoolInfo basePoolInfo)
        : base(basePoolInfo)
    {
        SubProviders = new List<Provider>();
    }
}