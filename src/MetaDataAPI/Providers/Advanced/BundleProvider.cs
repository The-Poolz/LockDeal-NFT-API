using System.Text;
using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Advanced;

public class BundleProvider : IProvider
{
    private readonly BigInteger poolId;
    private readonly BigInteger lastSubPoolId;
    private readonly byte decimals;
    public byte ParametersCount => 1;
    public List<Erc721Attribute> Attributes { get; }

    public BundleProvider(BigInteger poolId, byte decimals, IReadOnlyList<BigInteger> values)
    {
        this.poolId = poolId;
        lastSubPoolId = values[0];
        this.decimals = decimals;
        Attributes = new List<Erc721Attribute>();
        for (var id = poolId + 1; id <= lastSubPoolId; id++)
        {
            var providerAttributes = AttributesService.GetProviderAttributes(id, decimals);

            Attributes.AddRange(providerAttributes.Select(attribute => 
            attribute.IncludeUnderscoreForTraitType(id)));
        }
    }

    public string GetDescription(string token)
    {
        var descriptionBuilder = new StringBuilder().AppendLine("This NFT orchestrates a series of sub-pools to enable sophisticated asset management strategies. The following are the inner pools under its governance:");

        for (var id = poolId + 1; id <= lastSubPoolId; id++)
        {
            var metadata = RpcCaller.GetMetadata(id);
            var parser = new MetadataParser(metadata);

            var provider = ProviderFactory.Create(parser.GetProviderAddress(), poolId, decimals, parser.GetProviderParameters().ToArray());

            descriptionBuilder.AppendLine($"- {id}: {provider.GetDescription(token)}");
        }

        return descriptionBuilder.ToString();
    }
}