using System.Numerics;
using System.Text;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class BundleProvider : DealProvider
{
    public BundleProvider(BasePoolInfo basePoolInfo)
    : base(basePoolInfo)
    {
        SubProviders = Range(PoolInfo.PoolId + 1, LastPoolId)
                       .Select(poolId => basePoolInfo.Factory.Create(poolId));
    }
    public override string ProviderName => nameof(BundleProvider);
    public override string Description
    {
        get
        {
            var descriptionBuilder = new StringBuilder()
                .AppendLine("This NFT orchestrates a series of sub-pools to enable sophisticated asset management strategies." +
                $" The following are the inner pools under its governance that holds total {LeftAmount} {PoolInfo.Token}:");

            return SubProviders.Aggregate(descriptionBuilder, (sb, item) =>
                sb.AppendLine($"{item.PoolInfo.PoolId}: {item.ToString()}")).ToString();
        }
    }

    public override IEnumerable<Erc721Attribute> Attributes => base.Attributes.Concat(
        SubProviders
            .SelectMany(p => p.Attributes.Select(attr => new { p.PoolInfo.PoolId, Attribute = attr }))
            .Where(pa => IncludeTypes.Contains(pa.Attribute.TraitType))
            .Select(pa => pa.Attribute.IncludeUnderscoreForTraitType(pa.PoolId)));

    internal static List<string> IncludeTypes = new()
    { "LeftAmount", "StartTime", "FinishTime", "StartAmount", "ProviderName" };
    internal BigInteger LastPoolId => PoolInfo.Params[1];
    internal IEnumerable<Provider> SubProviders { get; }
    internal static IEnumerable<BigInteger> Range(BigInteger first, BigInteger last)
    {
        for (var i = first; i <= last; i++)
            yield return i;
    }
}