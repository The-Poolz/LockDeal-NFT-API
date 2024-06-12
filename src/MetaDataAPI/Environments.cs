using EnvironmentManager.Attributes;

namespace MetaDataAPI;

public enum Environments
{
    [EnvironmentVariable(isRequired: true)]
    LOCK_DEAL_NFT_ADDRESS,
    [EnvironmentVariable(isRequired: true)]
    RPC_URL,
    [EnvironmentVariable(isRequired: true)]
    NFT_HTML_ENDPOINT,
    [EnvironmentVariable(isRequired: true)]
    HTML_TO_IMAGE_ENDPOINT_TEMPLATE,
    [EnvironmentVariable(isRequired: true)]
    TLY_API_KEY
}