using Net.Urlify;
using System.Numerics;
using Net.Urlify.Attributes;
using MetaDataAPI.Models.Extension;
using EnvironmentManager.Extensions;
using MetaDataAPI.ImageGeneration.UrlifyModels.Types;

namespace MetaDataAPI.ImageGeneration.UrlifyModels;

public class BaseUrlifyModel : Urlify
{
    [QueryStringProperty("name")]
    public string Name { get; set; }

    [QueryStringProperty("id")]
    public BigInteger Id { get; set; }

    [QueryStringProperty("tA")]
    public QueryStringToken MainCoinToken { get; set; }

    public BaseUrlifyModel(PoolInfo poolInfo) : base((string)Environments.NFT_HTML_ENDPOINT.Get())
    {
        Name = poolInfo.Name;
        Id = poolInfo.PoolId;
        MainCoinToken = new QueryStringToken(poolInfo.Erc20Token.Name, "Left Amount", poolInfo.LeftAmount);
    }
}