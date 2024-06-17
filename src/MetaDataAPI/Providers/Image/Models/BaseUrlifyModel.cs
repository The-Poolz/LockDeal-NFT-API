using Net.Urlify;
using System.Numerics;
using Net.Urlify.Attributes;
using EnvironmentManager.Extensions;
using MetaDataAPI.Providers.PoolInformation;

namespace MetaDataAPI.Providers.Image.Models;

public class BaseUrlifyModel : Urlify
{
    [QueryStringProperty("name")]
    public string Name { get; set; }

    [QueryStringProperty("id")]
    public BigInteger Id { get; set; }

    [QueryStringProperty("tA")]
    public QueryStringToken Token { get; set; }

    public BaseUrlifyModel(PoolInfo poolInfo) : base((string)Environments.NFT_HTML_ENDPOINT.Get())
    {
        Name = poolInfo.Name;
        Id = poolInfo.PoolId;
        Token = new QueryStringToken(poolInfo.Erc20Token.Name, "Left Amount", poolInfo.LeftAmount);
    }
}