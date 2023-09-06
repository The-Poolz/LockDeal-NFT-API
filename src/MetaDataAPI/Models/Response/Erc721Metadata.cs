﻿using Newtonsoft.Json;

namespace MetaDataAPI.Models.Response;

public class Erc721Metadata
{
    public Erc721Metadata(BasePoolInfo poolInfo)
    {
        Name = "Lock Deal NFT Pool: " + poolInfo.PoolId;
        Attributes = poolInfo.Provider.Attributes;
        Image = @"https://nft.poolz.finance/test/image?id=" + poolInfo.PoolId;
        Description = poolInfo.Provider.GetDescription(poolInfo.Token.Address);
    }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("image")]
    public string Image { get; set; }

    [JsonProperty("attributes")]
    public List<Erc721Attribute> Attributes { get; set; }
}
