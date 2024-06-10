﻿using MetaDataAPI.Utils;
using Net.Urlify.Attributes;
using MetaDataAPI.Models.Extension;

namespace MetaDataAPI.ImageGeneration.UrlifyModels;

public class LockDealUrlifyModel : BaseUrlifyModel
{
    [QueryStringProperty("Start Time")]
    public string StartTime { get; set; }

    public LockDealUrlifyModel(PoolInfo poolInfo) : base(poolInfo)
    {
        StartTime = $"{TimeUtils.FromUnixTimestamp((long)poolInfo.Params[1]):g}";
    }
}