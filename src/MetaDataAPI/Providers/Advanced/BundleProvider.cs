using System.Text;
using System.Numerics;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Advanced;

public class BundleProvider : IProvider
{
    private readonly BigInteger lastSubPoolId;
    public byte ParametersCount => 1;
    public List<Erc721Attribute> Attributes { get; }
    public BasePoolInfo PoolInfo { get; }

    public BundleProvider(BasePoolInfo basePoolInfo)
    {
        PoolInfo = basePoolInfo;
        lastSubPoolId = basePoolInfo.Params[0];
        Attributes = new List<Erc721Attribute>();
        for (var id = PoolInfo.PoolId + 1; id <= lastSubPoolId; id++)
        {
            var providerAttributes = ProviderFactory.Create(id).Attributes;

            Attributes.AddRange(providerAttributes.Select(attribute => 
            attribute.IncludeUnderscoreForTraitType(id)));
        }
    }

    public string GetDescription()
    {
        var descriptionBuilder = new StringBuilder().AppendLine("This NFT orchestrates a series of sub-pools to enable sophisticated asset management strategies. The following are the inner pools under its governance:");

        for (var id = PoolInfo.PoolId + 1; id <= lastSubPoolId; id++)
        {
            var description = ProviderFactory.Create(id).GetDescription();

            descriptionBuilder.AppendLine($"- {id}: {description}");
        }

        return descriptionBuilder.ToString();
    }
}