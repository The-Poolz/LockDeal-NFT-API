using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public interface IProvider
{
    public BasePoolInfo PoolInfo { get; }
    public List<Erc721Attribute> Attributes { get; }
    public string GetDescription();
    public Erc721Metadata GetErc721Metadata()
    {
        var name = "Lock Deal NFT Pool: " + PoolInfo.PoolId;
        var image = @"https://nft.poolz.finance/test/image?id=" + PoolInfo.PoolId;
        return new Erc721Metadata(name, GetDescription(), image, Attributes);
    }
}
