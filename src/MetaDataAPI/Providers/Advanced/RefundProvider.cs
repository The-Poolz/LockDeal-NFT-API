using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers.Advanced;

public class RefundProvider : IProvider
{
    private readonly BigInteger poolId;
    public byte ParametersCount => 2;

    public RefundProvider(BigInteger poolId)
    {
        this.poolId = poolId;
    }

    public IEnumerable<Erc721Attribute> GetAttributes(params BigInteger[] values)
    {
        var attributes = new List<Erc721Attribute>
        {
            new("RateToWei", values[1], "number"),
            new("MainCoin", GetMainCoin()),
            new("Token", GetToken())
        };
        attributes.AddRange(AttributesService.GetProviderAttributes(poolId + 1));

        return attributes;
    }

    private string GetMainCoin()
    {
        var metadata = RpcCaller.GetMetadata(poolId + 1);
        return new MetadataParser(metadata).GetTokenAddress();
    }

    private string GetToken()
    {
        var metadata = RpcCaller.GetMetadata(poolId + 2);
        return new MetadataParser(metadata).GetTokenAddress();
    }
}