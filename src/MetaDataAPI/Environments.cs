using EnvironmentManager.Attributes;

namespace MetaDataAPI;

public enum Environments
{
    [EnvironmentVariable(isRequired: true)]
    LOCK_DEAL_NFT_ADDRESS,
    [EnvironmentVariable(isRequired: true)]
    RPC_URL,
    [EnvironmentVariable(isRequired: true)]
    NFT_HTML_ENDPOINT
}