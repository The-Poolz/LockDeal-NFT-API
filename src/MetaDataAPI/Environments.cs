﻿using EnvironmentManager.Attributes;

namespace MetaDataAPI;

public enum Environments
{
    [EnvironmentVariable(isRequired: true)]
    NFT_HTML_ENDPOINT,
    [EnvironmentVariable(isRequired: true)]
    HTML_TO_IMAGE_ENDPOINT,
    [EnvironmentVariable(isRequired: true)]
    TLY_API_KEY
}